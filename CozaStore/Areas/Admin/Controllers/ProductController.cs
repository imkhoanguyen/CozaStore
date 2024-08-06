using CozaStore.Entities;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductImageService _productImageService;
        public ProductController(IUnitOfWork unitOfWork, IProductImageService productImageService)
        {
            _unitOfWork = unitOfWork;
            _productImageService = productImageService;
        }
        public async Task<IActionResult> Index(string searchString, int page)
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync(searchString, page);
            return View(products);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> subCategoryList = _unitOfWork.SubCategoryRepository.GetAllSubCategories()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });

            ViewBag.SubCategories = subCategoryList;

            IEnumerable<SelectListItem> productStatusList = Enum.GetValues(typeof(ProductStatus))
               .Cast<ProductStatus>()
               .Select(status => new SelectListItem
               {
                   Text = status.ToString(),
                   Value = ((int)status).ToString(),
                   Selected = status == ProductStatus.Private
               });

            ViewBag.ProductStatuses = productStatusList;
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
                        var resultUpload = await _productImageService.AddImageAsync(img);
                        if (resultUpload.Error != null)
                        {
                            TempData["error"] = "Error uploading photo. Try again!";
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
            IEnumerable<SelectListItem> subCategoryList = _unitOfWork.SubCategoryRepository.GetAllSubCategories()
                .Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                });

            ViewBag.SubCategories = subCategoryList;

            IEnumerable<SelectListItem> productStatusList = Enum.GetValues(typeof(ProductStatus))
               .Cast<ProductStatus>()
               .Select(status => new SelectListItem
               {
                   Text = status.ToString(),
                   Value = ((int)status).ToString(),
                   Selected = status == ProductStatus.Private
               });

            ViewBag.ProductStatuses = productStatusList;

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
                } else
                {
                    TempData["success"] = "The variant has been deleted successfully.";
                    return Ok();
                }
            }
            return BadRequest("Something wrong");
        }
    }
}
