﻿using CozaStore.Data;
using CozaStore.Data.Enum;
using CozaStore.Interfaces;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using Stripe.Climate;
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
            vm.Order = new Models.Order();
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

            if (await _unitOfWork.Complete())
                return RedirectToAction(nameof(Index));

            return RedirectToAction("GetBadRequest", "Buggy", new { message = "Problems with increasing product quantity" });
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cartFromDb = await _unitOfWork.ShoppingCartRepository.GetShoppingCartAsync(cartId);

            if (cartFromDb.Count <= 1)
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

            // check delete & qty
            foreach (var cartItem in shoppingCartList)
            {
                if(cartItem.Product.IsDelete)
                {
                    TempData["error"] = cartItem.Product.Name + " has been deleted";
                    return RedirectToAction(nameof(Index));
                }
                
                if(cartItem.Product.Variants != null)
                {
                    foreach(var variant in cartItem.Product.Variants)
                    {
                        if(variant.SizeId == cartItem.SizeId && variant.ColorId == cartItem.ColorId && variant.IsDelete)
                        {
                            TempData["error"] = "Product or variant: " + cartItem.Product.Name + " has been deleted";
                            return RedirectToAction(nameof(Index));
                        }

                        if(variant.SizeId == cartItem.SizeId && variant.ColorId == cartItem.ColorId && variant.Quantity < cartItem.Count)
                        {
                            TempData["error"] = "Insufficient quantity of products: " + cartItem.Product.Name;
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }

            var user = await _context.AppUser
                         .Include(x => x.AddressList)
                         .FirstOrDefaultAsync(x => x.Id == userId);
            var listAddress = user.AddressList;

            var vm = new ShopingCartVM();
            vm.ShoppingCartList = shoppingCartList.ToList();
            vm.Order = new Models.Order();
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

        [HttpPost]
        public async Task<IActionResult> Checkout(ShopingCartVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartList = await _unitOfWork.ShoppingCartRepository.GetAllAsync(userId);
            var shippingMethod = await _unitOfWork.ShippingRepository.GetAsync(vm.Order.ShippingMethodId);

            var order = new Models.Order
            {
                Description = vm.Order.Description,
                FullName = vm.Order.FullName,
                Phone = vm.Order.Phone,
                SpecificAddress = vm.Order.SpecificAddress,
                ShippingMethodId = shippingMethod.Id,
                ShippingFee = shippingMethod.Cost,
                PaymentMethod = vm.Order.PaymentMethod,
                UserId = userId,
                OrderStatus = (int)OrderStatus.Unconfirmed,
                PaymentStatus = (int)PaymentStatus.Pending,
            };

            foreach (var item in shoppingCartList.ToList())
            {
                item.Product = await _unitOfWork.ProductRepository.GetProductDetailAsync(item.ProductId);
                item.Size = await _unitOfWork.SizeRepository.GetSizeAsync(item.SizeId);
                item.Color = await _unitOfWork.ColorRepository.GetColorAsync(item.ColorId);

                var orderItem = new OrderItem
                {
                    Name = item.Product.Name,
                    Url = item.Product.Images.FirstOrDefault(x => x.IsMain).Url ?? item.Product.Images.FirstOrDefault().Url,
                    Size = item.Size.Name,
                    Color = item.Color.Name,
                    Price = item.Price,
                    Count = item.Count,
                };
                order.OrderItemList.Add(orderItem);
                order.SubTotal += item.GetTotal();
            }
            _unitOfWork.OrderRepository.Add(order);
            await _unitOfWork.Complete();
            if (order.PaymentMethod == 0) // payment without stripe
            {
                //delete shopping cart
                _unitOfWork.ShoppingCartRepository.DeleteAll(shoppingCartList.ToList());
                await _unitOfWork.Complete();
                return RedirectToAction(nameof(OrderSuccess), new { orderId = order.Id });
            }
            else // paymet with stripe
            {
                // Get domain
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                // Create Stripe session options
                var options = new SessionCreateOptions
                {
                    SuccessUrl = domain + $"cart/ordersuccess/?orderId={order.Id}",
                    CancelUrl = domain + "order-cancel",
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                };

                // Add items to the session
                foreach (var orderItem in order.OrderItemList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(orderItem.Price) * 100,
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = orderItem.Name + " - " + orderItem.Color + " - " + orderItem.Size + " x " + orderItem.Count,
                                Images = new List<string> { "https://res.cloudinary.com/dh1zsowbp/image/upload/v1724649479/455275001_371764102635181_8003846153934311941_n_z3jo1z.jpg" }
                            }
                        },
                        Quantity = orderItem.Count,
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                // Add shipping fee to the session
                var shippingLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(order.ShippingFee) * 100, // Convert to cents
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Shipping Fee",
                        }
                    },
                    Quantity = 1,
                };

                options.LineItems.Add(shippingLineItem);

                // Create Stripe session
                var service = new SessionService();
                Session session = service.Create(options);

                //update session to db
                _unitOfWork.OrderRepository.UpdateStripePaymentId(order.Id, session.Id, session.PaymentIntentId);
                await _unitOfWork.Complete();
                // Redirect to Stripe Checkout
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
        }

        public async Task<IActionResult> OrderSuccess(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            if(order == null)
                return RedirectToAction("GetNotFound", "Buggy", new { message = "Order not found" });
            if (order.PaymentMethod == 1)
            {
                var service = new SessionService();
                Session session = service.Get(order.StripeSessionId);

                if(session.PaymentStatus == "paid")
                {
                    _unitOfWork.OrderRepository.UpdateStripePaymentId(order.Id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderRepository.UpdatePaymentStatus(orderId, (int)PaymentStatus.Paid);
                    
                    //delete shopping cart & update quantity product 
                    var shoppingCartList = await _unitOfWork.ShoppingCartRepository.GetAllAsync(order.UserId);
                    foreach(var cartItem in shoppingCartList.ToList())
                    {
                        //update
                        var variant = await _unitOfWork.ProductRepository.GetVariantOfProductAsync(cartItem.ProductId, cartItem.ColorId, cartItem.SizeId);
                        if (variant != null)
                            _unitOfWork.VariantRepository.UpdateQuantity(variant.Id, cartItem.Count);
                        else
                            _unitOfWork.ProductRepository.UpdateQuantity(cartItem.ProductId, cartItem.Count);

                        //delete
                        _unitOfWork.ShoppingCartRepository.Delete(cartItem);
                    }
                    
                    await _unitOfWork.Complete();
                }
            }

            return View(order);
        }
    }
}
