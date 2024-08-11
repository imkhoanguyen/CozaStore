using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Services;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CozaStore.Helpers.Enum;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _imageService;
        private const int _pageSize = 10;
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
            if (!string.IsNullOrEmpty(productParams.SelectedCategoryString))
            {
                productParams.SelectedCategory = productParams.SelectedCategoryString
                    .Split(',')
                    .Select(int.Parse)
                    .ToList();
            }

            ViewData["CurrentSort"] = productParams.SortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(productParams.SortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = productParams.SortOrder == "id" ? "id_desc" : "id";
            ViewData["PriceSortParm"] = productParams.SortOrder == "price" ? "price_desc" : "price";
            ViewData["StatusSortParm"] = productParams.SortOrder == "status" ? "status_desc" : "status";
            ViewData["CurrentFilter"] = productParams.SearchString;

            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync(productParams);

            var vm = new ProductVM
            {
                Products = products,
                CurrentSort = productParams.SortOrder,
                NameSortParm = ViewData["NameSortParm"].ToString(),
                IdSortParm = ViewData["IdSortParm"].ToString(),
                PriceSortParm = ViewData["PriceSortParm"].ToString(),
                StatusSortParm = ViewData["StatusSortParm"].ToString(),
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
                            TempData["error"] = "Error uploading image. Try again!";
                            return View();
                        }
                        var productImg = new Image
                        {
                            Url = resultUpload.SecureUri.AbsoluteUri,
                            PublicId = resultUpload.PublicId,
                        };

                        vm.Product.Images.Add(productImg);
                        _unitOfWork.ProductRepository.AddProduct(vm.Product);
                    }
                } else
                {
                    return View();
                }

                if(vm.SelectedCategory != null && vm.SelectedCategory.Count > 0)
                {
                   foreach(int categoryId in vm.SelectedCategory)
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
            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(id);
            if (product == null) return NotFound();
            ProductCreateVM vm = new ProductCreateVM();
            vm.Product = product;
            vm.CategoryList = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync();
            vm.SelectedCategory = product.ProductCategories.Select(x=>x.CategoryId).ToList();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductCreateVM vm)
        {
            if (ModelState.IsValid && vm.Product.Id > 0)
            {
                var productFromDb = await _unitOfWork.ProductRepository.GetProductAsync(vm.Product.Id);
                if(productFromDb == null) return NotFound();
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

        [HttpPost]
        public async Task<IActionResult> ToggleProductStatus(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            if (product == null) return NotFound();
            int status = product.Status;
            _unitOfWork.ProductRepository.ToggleProductStatus(product);
            if (await _unitOfWork.Complete())
            {
                if (status == (int)ProductStatus.Deleted)
                {
                    TempData["success"] = "The variant has been recoverd successfully.";
                    return Ok();
                }
                else
                {
                    TempData["success"] = "The variant has been deleted successfully.";
                    return Ok();
                }
            }
            return BadRequest("Something wrong");
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(id);
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
            return View("variantcreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(VariantCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                var product = await _unitOfWork.ProductRepository.GetProductAsync(vm.Variant.ProductId);
                if (product == null)
                {
                    TempData["error"] = "Something wrong";
                    return BadRequest();
                }

                product.Variants.Add(vm.Variant);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The variant has been created successfully.";
                    return RedirectToAction(nameof(Detail), new { id = vm.Variant.ProductId });
                }
            }
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVariant(int variantId)
        {
            var variant = await _unitOfWork.VariantRepository.GetVariantAsync(variantId);
            if (variant == null) return NotFound();
            
            _unitOfWork.VariantRepository.DeleteVariant(variant);
            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "The variant has been deleted successfully.";
                return Ok();
            }
            return BadRequest("Something wrong");
        }


        [HttpPost]
        public async Task<IActionResult> SetMainImage(int productId, int imgId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            if (product == null) return NotFound();

            var img = product.Images.FirstOrDefault(x => x.Id == imgId);
            if (img == null || img.IsMain) return BadRequest("Can not use this as main image");

            var currentMain = product.Images.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;

            img.IsMain = true;

            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "The img has been set main successfully.";
                return Ok();
            }
            return BadRequest("Problem set main image");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteImage(int productId, int imgId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductAsync(productId);
            if (product == null) return NotFound();

            var img = product.Images.FirstOrDefault(x => x.Id == imgId);
            var listImg = product.Images.ToList();
            if (img == null || img.IsMain || listImg.Count == 1) return BadRequest("This image cannot be deleted");

            if (img.ProductId != null)
            {
                var result = await _imageService.DeleteImageAsync(img.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            product.Images.Remove(img);

            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "The img has been deleted successfully.";
                return Ok();
            }

            return BadRequest("Problem deleting image");
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
            if (vm != null && vm.ProductId > 0)
            {
                var product = await _unitOfWork.ProductRepository.GetProductAsync(vm.ProductId);
                if (product == null) return BadRequest();

                if (vm.Images.Count == 0)
                {
                    ModelState.AddModelError("Images", "You have not uploaded the image file");
                    return View();
                }

                foreach (var img in vm.Images)
                {
                    var resultUpload = await _imageService.AddImageAsync(img);
                    if (resultUpload.Error != null)
                    {
                        TempData["error"] = "Error uploading image. Try again!";
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
