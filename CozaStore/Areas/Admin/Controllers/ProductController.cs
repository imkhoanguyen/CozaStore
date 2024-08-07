using CozaStore.Models;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Services;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public async Task<IActionResult> Index(string sortOrder, string searchString, int selectedCategory,
            int selectedStatus, string selectedPriceRange, int selectedColor, int selectedSize, int page = 1)
        {
            var categories = _unitOfWork.CategoryRepository.GetAllCategories();
            var colors = _unitOfWork.ColorRepository.GetAllColors();
            var sizes = _unitOfWork.SizeRepository.GetAllSizes();

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["PriceSortParm"] = sortOrder == "price" ? "price_desc" : "price";
            ViewData["StatusSortParm"] = sortOrder == "status" ? "status_desc" : "status";
            ViewData["CurrentFilter"] = searchString;

            var products = await _unitOfWork.ProductRepository
                .GetAllProductsAsync(sortOrder, searchString, selectedCategory,
             selectedStatus, selectedPriceRange, selectedSize, selectedColor, page, _pageSize);

            var vm = new ProductVM
            {
                Products = products,
                CurrentSort = sortOrder,
                NameSortParm = ViewData["NameSortParm"].ToString(),
                IdSortParm = ViewData["IdSortParm"].ToString(),
                PriceSortParm = ViewData["PriceSortParm"].ToString(),
                StatusSortParm = ViewData["StatusSortParm"].ToString(),
                CurrentFilter = searchString,
                SelectedCategory = selectedCategory,
                SelectedStatus = selectedStatus,
                SelectedColor = selectedColor,
                SelectedSize = selectedSize,
                SelectedPriceRange = selectedPriceRange,
                Categories = categories.ToList(),
                Sizes = sizes.ToList(),
                Colors = colors.ToList(),
                PriceRanges = SD.PriceRangeList
            };

            return View(vm);
        }

        public IActionResult Create()
        {

            ViewBag.Categories = _unitOfWork.CategoryRepository.GetAllCategories();
            ViewBag.ProductStatuses = SD.ProductStatusList;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ImagesFile != null && product.ImagesFile.Count > 0)
                {
                    foreach (var img in product.ImagesFile)
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

                    _unitOfWork.ProductRepository.AddProduct(product);

                    if (await _unitOfWork.Complete())
                    {
                        TempData["success"] = "The product has been created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = _unitOfWork.CategoryRepository.GetAllCategories();
            ViewBag.ProductStatuses = SD.ProductStatusList;

            var product = await _unitOfWork.ProductRepository.GetProductAsync(id);
            if (product == null) return RedirectToAction("Error", "Dashboard");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid && product.Id > 0)
            {
                _unitOfWork.ProductRepository.UpdateProduct(product);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The product has been edited successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
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
            var product = await _unitOfWork.ProductRepository.GetProductAsync(id);
            return View(product);
        }

        public IActionResult CreateVariant(int productId)
        {

            ViewBag.Colors = _unitOfWork.ColorRepository.GetAllColors();

            ViewBag.Sizes = _unitOfWork.SizeRepository.GetAllSizes();


            ViewBag.VariantStatuses = SD.VariantStatusList;

            Variant variant = new Variant();
            variant.ProductId = productId;
            return View("variantcreate", variant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(Variant variant)
        {
            if (ModelState.IsValid)
            {
                var product = await _unitOfWork.ProductRepository.GetProductAsync(variant.ProductId);
                if (product == null)
                {
                    TempData["error"] = "Something wrong";
                    return BadRequest();
                }

                product.Variants.Add(variant);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The variant has been created successfully.";
                    return RedirectToAction(nameof(Detail), new { id = variant.ProductId });
                }
            }
            return View();
        }

        public async Task<IActionResult> EditVariant(int productId, int variantId)
        {
            ViewBag.Colors = _unitOfWork.ColorRepository.GetAllColors();

            ViewBag.Sizes = _unitOfWork.SizeRepository.GetAllSizes();


            ViewBag.VariantStatuses = SD.VariantStatusList;

            var variant = await _unitOfWork.VariantRepository.GetVariantAsync(variantId);
            if (variant == null || variant.ProductId != productId) return NotFound();

            return View("variantedit", variant);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariant(Variant variant)
        {
            if (ModelState.IsValid && variant.Id > 0 && variant.ProductId > 0)
            {
                _unitOfWork.VariantRepository.UpdateVariant(variant);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The variant has been edited successfully.";
                    return RedirectToAction(nameof(Detail), new { id = variant.ProductId });
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatusVariant(int variantId)
        {
            var variant = await _unitOfWork.VariantRepository.GetVariantAsync(variantId);
            if (variant == null) return NotFound();
            int status = variant.Status;
            _unitOfWork.VariantRepository.ToggleStatusVariant(variant);
            if (await _unitOfWork.Complete())
            {
                if (status == (int)VariantStatus.Deleted)
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
                return RedirectToAction(nameof(Detail), new { id = product.Id });
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
                return RedirectToAction(nameof(Detail), new { id = product.Id });
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
