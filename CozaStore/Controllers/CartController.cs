using CozaStore.Data;
using CozaStore.Interfaces;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CozaStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DataContext _context;

        public CartController(IUnitOfWork unitOfWork, DataContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartList = await _unitOfWork.ShoppingCartRepository.GetAllAsync(userId);

            ShopingCartVM vm = new ShopingCartVM();
            vm.ShoppingCartList = shoppingCartList.ToList();
            vm.Order = new Order();
            foreach (var cartItem in vm.ShoppingCartList)
            {
                cartItem.Product = await _unitOfWork.ProductRepository.GetProductDetailAsync(cartItem.ProductId);
                cartItem.Size = await _unitOfWork.SizeRepository.GetSizeAsync(cartItem.SizeId);
                cartItem.Color = await _unitOfWork.ColorRepository.GetColorAsync(cartItem.ColorId);

                vm.Order.SubTotal += cartItem.GetTotal();
            }

            return View(vm);
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cartFromDb = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAsync(cartId);

            decimal singalPrice = cartFromDb.Price / cartFromDb.Count;

            cartFromDb.Count += 1;
            cartFromDb.Price += singalPrice;

            if(await _unitOfWork.Complete())
                return RedirectToAction(nameof(Index));
            
            return RedirectToAction("GetBadRequest" , "Buggy", new { message = "Problems with increasing product quantity" });
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartFromDb = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAsync(cartId);

            if(cartFromDb.Count <= 1)
                _unitOfWork.ShoppingCartRepository.Delete(cartFromDb);
            else
            {
                decimal singalPrice = cartFromDb.Price / cartFromDb.Count;
                cartFromDb.Count -= 1;
                cartFromDb.Price -= singalPrice;
            }

            if (await _unitOfWork.Complete())
                return RedirectToAction(nameof(Index));

            return RedirectToAction("GetBadRequest", "Buggy", new { message = "Problems with reducing product quantity" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cartId)
        {
            var cartFromDb = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAsync(cartId);
            if (cartFromDb == null)
                return Json(new { success = false, message = "Cart item not found" });

            _unitOfWork.ShoppingCartRepository.Delete(cartFromDb);
            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "Remove product from cart successfully" });

            return Json(new { success = false, message = "Remove product from cart fail" });
        }

        public async Task<IActionResult> Checkout(int cartId)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartList = await _unitOfWork.ShoppingCartRepository.GetAllAsync(userId);

            var user = await _context.AppUser
                         .Include(x => x.AddressList)
                         .FirstOrDefaultAsync(x => x.Id == userId);
            var listAddress = user.AddressList;

            var vm = new ShopingCartVM();
            vm.ShoppingCartList = shoppingCartList.ToList();
            vm.Order = new Order();
            foreach (var cartItem in vm.ShoppingCartList)
            {
                cartItem.Product = await _unitOfWork.ProductRepository.GetProductDetailAsync(cartItem.ProductId);
                cartItem.Size = await _unitOfWork.SizeRepository.GetSizeAsync(cartItem.SizeId);
                cartItem.Color = await _unitOfWork.ColorRepository.GetColorAsync(cartItem.ColorId);

                vm.Order.SubTotal += cartItem.GetTotal();
            }
            vm.ListAddress = listAddress;
            var listShippingMethod = await _unitOfWork.ShippingRepository.GetAllAsync();
            vm.ListShippingMethod = listShippingMethod.ToList();
            return View(vm);
        }
    }
}
