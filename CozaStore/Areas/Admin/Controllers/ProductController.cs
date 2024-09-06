using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        public ProductController(IUnitOfWork unitOfWork, IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _imageService = imageService;
        }
        public async Task<IActionResult> Index(ProductParams productParams)
        {
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            var colors = await _unitOfWork.ColorRepository.GetAllColorsAsync();
            var sizes = await _unitOfWork.SizeRepository.GetAllSizesAsync();

            // get category list from url
            if (!string.IsNullOrEmpty(productParams.SelectedCategoryString))
            {
                productParams.SelectedCategory = productParams.SelectedCategoryString
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
            }

            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync(productParams);

            var vm = new ProductVM
            {
                Products = products,
                CurrentSort = productParams.SortOrder,
                NameSortParm = productParams.SortOrder == "name" ? "name_desc" : "name",
                IdSortParm = productParams.SortOrder == "id" ? "id_desc" : "id",
                PriceSortParm = productParams.SortOrder == "price" ? "price_desc" : "price",
                StatusSortParm = productParams.SortOrder == "status" ? "status_desc" : "status",
                CurrentFilter = productParams.SearchString,
                SelectedColor = productParams.SelectedColor,
                SelectedSize = productParams.SelectedSize,
                SelectedPriceRange = productParams.SelectedPriceRange,
                SelectedCategory = productParams.SelectedCategory,
                CategoryList = categories.ToList(),
                SizeList = sizes.ToList(),
                ColorList = colors.ToList(),
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            ProductCreateVM vm = new ProductCreateVM();
            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            if (vm.CategoryList.Count() < 1)
                return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Category is empty. Please add category and come back!!!" });
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.ImagesFile != null && vm.ImagesFile.Count > 0)
                {
                    // add product img
                    foreach (var img in vm.ImagesFile)
                    {
                        var resultUpload = await _imageService.AddImageAsync(img);
                        if (resultUpload.Error != null)
                        {
                            TempData["error"] = resultUpload.Error.Message;
                            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
                            return View(vm);
                        }
                        var productImg = new Image
                        {
                            Url = resultUpload.SecureUri.AbsoluteUri,
                            PublicId = resultUpload.PublicId,
                        };

                        vm.Product.Images.Add(productImg);
                        _unitOfWork.ProductRepository.AddProduct(vm.Product);
                    }
                }

                if (vm.SelectedCategory != null && vm.SelectedCategory.Count > 0)
                {
                    foreach (int categoryId in vm.SelectedCategory)
                    {
                        var productCategory = new ProductCategory
                        {
                            ProductId = vm.Product.Id,
                            CategoryId = categoryId
                        };
                        vm.Product.ProductCategories.Add(productCategory);
                    }
                }

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The product has been created successfully.";
                    return RedirectToAction(nameof(Index));
                }

            }
            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            return View(vm);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(id);
            if (product == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Product not found!!!" });
            ProductCreateVM vm = new ProductCreateVM();
            vm.Product = product;
            vm.SelectedCategory = product.ProductCategories.Select(x => x.CategoryId).ToList();
            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            if (vm.CategoryList.Count() < 1)
                return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Category is empty. Please add category and come back!!!" });
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateVM vm)
        {
            if (ModelState.IsValid && vm.Product.Id > 0)
            {
                var productFromDb = await _unitOfWork.ProductRepository.GetProductAsync(vm.Product.Id);
                if (productFromDb == null)
                    return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Product not found!!!" });
                var existingCategoryIds = productFromDb.ProductCategories.Select(pc => pc.CategoryId).ToList();

                var categoriesToRemove = existingCategoryIds.Except(vm.SelectedCategory).ToList();
                var categoriesToAdd = vm.SelectedCategory.Except(existingCategoryIds).ToList();

                // delete category
                foreach (int categoryId in categoriesToRemove)
                {
                    var productCategory = productFromDb.ProductCategories.FirstOrDefault(pc => pc.CategoryId == categoryId);
                    if (productCategory != null)
                    {
                        productFromDb.ProductCategories.Remove(productCategory);
                    }
                }

                // add category
                foreach (int categoryId in categoriesToAdd)
                {
                    var productCategory = new ProductCategory
                    {
                        CategoryId = categoryId,
                        ProductId = productFromDb.Id,
                    };
                    productFromDb.ProductCategories.Add(productCategory);
                }

                _unitOfWork.ProductRepository.UpdateProduct(vm.Product);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The product has been edited successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            vm.SelectedCategory = vm.Product.ProductCategories.Select(x => x.CategoryId).ToList();
            return View(vm);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found" });

            _unitOfWork.ProductRepository.Delete(product);
            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "The product has been deleted successfully." });

            return Json(new { success = true, message = "Problem delete product" });
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(id);
            if (product == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Product not found!!!" });
            return View(product);
        }

        public async Task<IActionResult> CreateVariant(int productId)
        {
            Variant variant = new Variant();
            variant.ProductId = productId;

            VariantCreateVM vm = new VariantCreateVM()
            {
                ColorList = await _unitOfWork.ColorRepository.GetAllColorsAsync(),
                SizeList = await _unitOfWork.SizeRepository.GetAllSizesAsync(),
                Variant = variant
            };

            if (vm.ColorList.Count() < 1)
                return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Color is empty. Please add category and come back!!!" });

            if (vm.SizeList.Count() < 1)
                return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Size is empty. Please add category and come back!!!" });

            return View("variantcreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(VariantCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var product = await _unitOfWork.ProductRepository.GetProductAsync(vm.Variant.ProductId);
                if (product == null)
                    return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Product not found!!!" });

                product.Variants.Add(vm.Variant);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The variant has been created successfully.";
                    return RedirectToAction(nameof(Detail), new { id = vm.Variant.ProductId });
                }
            }
            vm.SizeList = await _unitOfWork.SizeRepository.GetAllSizesAsync();
            vm.ColorList = await _unitOfWork.ColorRepository.GetAllColorsAsync();
            return View();
        }

        public async Task<IActionResult> EditVariant(int productId, int variantId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);

            if (variant == null || variant.ProductId != product.Id) return NotFound();

            VariantCreateVM vm = new VariantCreateVM
            {
                Variant = variant,
                ColorList = await _unitOfWork.ColorRepository.GetAllColorsAsync(),
                SizeList = await _unitOfWork.SizeRepository.GetAllSizesAsync()
            };

            return View("variantedit", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariant(VariantCreateVM vm)
        {
            if (ModelState.IsValid && vm.Variant.Id > 0 && vm.Variant.ProductId > 0)
            {
                _unitOfWork.VariantRepository.UpdateVariant(vm.Variant);


                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The variant has been edited successfully.";
                    return RedirectToAction(nameof(Detail), new { id = vm.Variant.ProductId });
                }
            }
            vm.SizeList = await _unitOfWork.SizeRepository.GetAllSizesAsync();
            vm.ColorList = await _unitOfWork.ColorRepository.GetAllColorsAsync();
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVariant(int variantId)
        {
            var variant = await _unitOfWork.VariantRepository.GetVariantAsync(variantId);
            if (variant == null)
                return Json(new {success = false, message = "Variant not found!!!" });
           

            _unitOfWork.VariantRepository.DeleteVariant(variant);
            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "The variant has been deleted successfully." });

            return Json(new { success = false, message = "Problem delete variant!!!" });
        }


        [HttpPost]
        public async Task<IActionResult> SetMainImage(int productId, int imgId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            if (product == null)
            {
                TempData["error"] = "Product not found!!!";
                return NoContent();
            }

            var img = product.Images.FirstOrDefault(x => x.Id == imgId);
            if (img == null || img.IsMain)
            {
                TempData["error"] = "Can not use this as main image";
                return NoContent();
            }

            var currentMain = product.Images.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            img.IsMain = true;

            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "The img has been set main successfully.";
                return NoContent();
            }
            TempData["error"] = "Problem set main image!!!";
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int productId, int imgId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            if (product == null)
                return Json(new { success = false, message = "Product not found!!!" });


            var img = product.Images.FirstOrDefault(x => x.Id == imgId);
            var listImg = product.Images.ToList();
            if (img == null || img.IsMain || listImg.Count == 1)
                return Json(new { success = false, message = "This image cannot be delete!!!" });

            if (img.PublicId != null) // xoa tren cloud
            {
                var result = await _imageService.DeleteImageAsync(img.PublicId);
                if (result.Error != null)
                    return Json(new { success = false, message = result.Error.Message });
            }

            product.Images.Remove(img);

            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "The img has been deleted successfully." });
           
            return Json(new { success = false, message = "Problem deleting image" });
        }

        public IActionResult AddImages(int productId)
        {
            AddProductImageVM vm = new AddProductImageVM();
            vm.ProductId = productId;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddImages(AddProductImageVM vm)
        {
            if (!ModelState.IsValid) return View(vm); 
            if (vm != null && vm.ProductId > 0)
            {
                var product = await _unitOfWork.ProductRepository.GetProductAsync(vm.ProductId);
                if (product == null)
                    return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Product not found!!!" });

                foreach (var img in vm.Images)
                {
                    var resultUpload = await _imageService.AddImageAsync(img);
                    if (resultUpload.Error != null)
                    {
                        TempData["error"] = resultUpload.Error.Message;
                        vm.ProductId = product.Id;
                        return View();
                    }
                    var productImg = new Image
                    {
                        Url = resultUpload.SecureUri.AbsoluteUri,
                        PublicId = resultUpload.PublicId,
                    };

                    product.Images.Add(productImg);
                }

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The images has been added successfully.";
                    return RedirectToAction(nameof(Detail), new { id = vm.ProductId });
                }
            }
            return View(vm);
        }
    }
}
