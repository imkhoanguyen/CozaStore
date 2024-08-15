using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CozaStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int _pageNumber = 16;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        [HttpGet]
        public async Task<IActionResult> GetProductDetail(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(id);
            var vm = new ProductDetailVM
            {
                Id = product.Id,
                Name = product.Name,
                PriceSell = product.PriceSell,
                Quantity = product.Quantity,
                Description = product.Description,
                ColorList = product.Variants.Select(x=>x.Color),
                SizeList = product.Variants.Select(x=>x.Size),
                ImageList = product.Images.Select(x=>x.Url),
            };

            return Json(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableColors(int productId, int sizeId)
        {
            var colors = await _unitOfWork.ProductRepository.GetAvailableColorsAsync(productId, sizeId);

            return Json(colors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSizes(int productId, int colorId)
        {
            var sizes = await _unitOfWork.ProductRepository.GetAvailableSizesAsync(productId, colorId);

            return Json(sizes);
        }

        [HttpGet]
        public async Task<IActionResult> GetPrice(int productId, int sizeId, int colorId)
        {
            if (sizeId == 0 || colorId == 0)
                return Json(new { success = false, message = "You have not selected size and color, please try again." });

            var variant = await _unitOfWork.ProductRepository.GetPriceOfProductAsync(productId, colorId,sizeId);

            if(variant == null)
                return Json(new { success = false, message = "You chose too fast, try again." });
           
            return Json(variant.PriceSell);
        }
    }
}
