using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CozaStore.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int _pageSize = 8;
        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(ProductParams productParams)
        {
            productParams.PageSize = _pageSize;
            productParams.Shop = true;
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

        [HttpPost]
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
                ColorList = product.Variants.Select(x=>x.Color).Distinct(),
                SizeList = product.Variants.Select(x=>x.Size).Distinct(),
                ImageList = product.Images.Select(x=>x.Url),
            };

            return PartialView("_ProductDetailPartial", vm);
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableColors(int productId, int sizeId)
        {
            var colors = await _unitOfWork.ProductRepository.GetAvailableColorsAsync(productId, sizeId);

            return Json(colors);
        }

        [HttpPost]
        public async Task<IActionResult> GetAvailableSizes(int productId, int colorId)
        {
            var sizes = await _unitOfWork.ProductRepository.GetAvailableSizesAsync(productId, colorId);

            return Json(sizes);
        }

        [HttpPost]
        public async Task<IActionResult> GetPrice(int productId, int sizeId, int colorId, string currentSelect)
        {
            if (sizeId == 0 || colorId == 0)
                return Json(new { success = false, message = "You have not selected size and color, please try again." });

            var variant = await _unitOfWork.ProductRepository.GetVariantOfProductAsync(productId, colorId,sizeId);

            if(variant == null && currentSelect == "color") // getSize
                return Json(new { success = false, message = "No variant", select = "color" });

			if (variant == null && currentSelect == "size") // getSize
				return Json(new { success = false, message = "No variant", select = "size" });

			return Json(variant.PriceSell);
        }

        [HttpPost]
        [Authorize(Policy = ClaimStore.Cart_Add)]
        public async Task<IActionResult> AddToCart(int productId, int sizeId, int colorId, int count)
        {
            decimal price = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(productId);
            if(product == null)
                return Json(new { success = false, message = "Product notfound" });

            if(product.Variants.Count > 0 && sizeId ==0)
                return Json(new { success = false, message = "You have not selected a size" });

            if(product.Variants.Count > 0 &&  colorId == 0)
                return Json(new { success = false, message = "You have not selected a color" });


            if(product.Variants.Count > 0)
            {
                var variant = product.Variants.Where(x=> x.SizeId ==  sizeId && x.ColorId == colorId).FirstOrDefault();
                if(variant == null)
                    return Json(new { success = false, message = "Product not found.Please select another size and color" });

                if(variant.Quantity < count)
                    return Json(new { success = false, message = "Product is out of stock or not in sufficient quantity" });

                price = variant.PriceSell;
            } else
            {
                price = product.PriceSell;
            }

            var shoppingCart = new ShoppingCart
            {
                UserId = userId,
                ProductId = productId,
                Price = price,
                SizeId = sizeId,
                ColorId = colorId,
                Count = count,
            };

            var checkShoppingCart = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAsync(userId, 
                shoppingCart.ProductId, shoppingCart.SizeId, shoppingCart.ColorId);

            bool cartIncreaseUI = false;

            if(checkShoppingCart == null)
            {
                _unitOfWork.ShoppingCartRepository.Add(shoppingCart);
                cartIncreaseUI = true;
            } else
            {
                _unitOfWork.ShoppingCartRepository.UpdatteIncrease(shoppingCart);
            }

            if(await _unitOfWork.Complete())
            {
                return Json(new { success = true, message = "Product added to cart successfully", add = cartIncreaseUI });
            } else
                return Json(new { success = false, message = "Product added to cart failed" });
        }

        public async Task<IActionResult> Product(int productId, PaginationParams paginationParams)
        {
            var product = await _unitOfWork.ProductRepository.GetProductDetailAsync(productId);
            var vm = new ProductDetailVM
            {
                Id = product.Id,
                Name = product.Name,
                PriceSell = product.PriceSell,
                Quantity = product.Quantity,
                Description = product.Description,
                ColorList = product.Variants.Select(x => x.Color).Distinct(),
                SizeList = product.Variants.Select(x => x.Size).Distinct(),
                ImageList = product.Images.Select(x => x.Url),
                ReviewList = await _unitOfWork.ReviewRepository.GetAllReviewsAsync(product.Id, paginationParams)
            };
            return View(vm);
        }
    }
}
