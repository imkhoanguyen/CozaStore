using CozaStore.Entities;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
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
                            TempData["error"] = "Error uploading photo. Try again";
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

                    if(await _unitOfWork.Complete())
                    {
                        TempData["success"] = "The product has been created successfully.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }
    }
}
