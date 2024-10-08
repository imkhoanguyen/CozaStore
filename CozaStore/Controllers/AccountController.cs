﻿using CozaStore.Data;
using CozaStore.Data.Enum;
using CozaStore.Helpers;
using CozaStore.Hubs;
using CozaStore.Interfaces;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace CozaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly DataContext _context;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<OrderHub> _orderHub;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            DataContext context, IImageService imageService, IUnitOfWork unitOfWork, IHubContext<OrderHub> orderHub)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _orderHub = orderHub;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if (!ModelState.IsValid) return View();

            var appUser = vm.UserName.Contains("@")
             ? await _userManager.FindByEmailAsync(vm.UserName)
             : await _userManager.FindByNameAsync(vm.UserName);

            var result = await _signInManager.PasswordSignInAsync(appUser, vm.Password, vm.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                TempData["success"] = "Login success!!!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("Login", "Failed to login");
                return View();
            }
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();

            var user = new AppUser
            {
                UserName = vm.UserName,
                FullName = vm.FullName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Gender = vm.Gender,
                Image = "/img/avatar.png",
            };



            var result = await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                TempData["success"] = "Register successfully!";
                return RedirectToAction("Login", "Account");
            }
            AddErrors(result);
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ChangeInformation()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await _context.AppUser
                        .Include(x => x.AddressList)
                        .FirstOrDefaultAsync(x => x.Id == userId);


            var vm = new EditUserVM
            {
                AppUser = user,
                AddressList = user.AddressList
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeInformation(EditUserVM vm)
        {
            if(!ModelState.IsValid)
            {
                var user1 = await _context.AppUser
                        .Include(x => x.AddressList)
                        .FirstOrDefaultAsync(x => x.Id == vm.AppUser.Id);
                vm.AppUser = user1;
                vm.AddressList = user1.AddressList;
                return View(vm);
            }

            if (vm.AppUser.FullName.IsNullOrEmpty())
            {
                var user1 = await _context.AppUser
                        .Include(x => x.AddressList)
                        .FirstOrDefaultAsync(x => x.Id == vm.AppUser.Id);
                vm.AppUser = user1;
                vm.AddressList = user1.AddressList;
                ModelState.AddModelError("AppUser.FullName", "FullName is required");
                return View(vm);
            }

            if (vm.AppUser.PhoneNumber.IsNullOrEmpty())
            {
                var user1 = await _context.AppUser
                        .Include(x => x.AddressList)
                        .FirstOrDefaultAsync(x => x.Id == vm.AppUser.Id);
                vm.AppUser = user1;
                vm.AddressList = user1.AddressList;
                ModelState.AddModelError("AppUser.PhoneNumber", "PhoneNumber is required");
                return View(vm);
            }

            if (ModelState.IsValid)
            {
                var userFromDb = await _context.AppUser.Include(x => x.AddressList).FirstOrDefaultAsync(x => x.Id == vm.AppUser.Id);
                if (userFromDb == null)
                    RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "User not found!!!" });

                userFromDb.FullName = vm.AppUser.FullName;
                userFromDb.PhoneNumber = vm.AppUser.PhoneNumber;
                userFromDb.Gender = vm.AppUser.Gender;
                if (vm.Image != null)
                {
                    string flagPublicId = "";
                    if (!userFromDb.PublicId.IsNullOrEmpty())
                        flagPublicId = userFromDb.PublicId;

                    var resultUpload = await _imageService.AddImageAsync(vm.Image);
                    if (resultUpload.Error != null)
                    {
                        vm.AddressList = userFromDb.AddressList;
                        vm.AppUser = userFromDb;

                        TempData["error"] = resultUpload.Error.Message;
                        return View(vm);
                    }

                    userFromDb.Image = resultUpload.SecureUri.AbsoluteUri;
                    userFromDb.PublicId = resultUpload.PublicId;

                    if(!flagPublicId.IsNullOrEmpty()) // delete img on cloud
                    {
                        var resultDelete = await _imageService.DeleteImageAsync(flagPublicId);
                        if (resultDelete.Error != null)
                        {
                            vm.AddressList = userFromDb.AddressList;
                            vm.AppUser = userFromDb;
                            TempData["error"] = resultDelete.Error.Message;
                            return View(vm);
                        }
                    } 
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    vm.AddressList = userFromDb.AddressList;
                    vm.AppUser = userFromDb;
                    TempData["success"] = "Edit information successfully";
                    return View(vm);
                }
            }
            var user = await _context.AppUser
                        .Include(x => x.AddressList)
                        .FirstOrDefaultAsync(x => x.Id == vm.AppUser.Id);
            vm.AppUser = user;
            vm.AddressList = user.AddressList;
            TempData["error"] = "Problem with edit information";
            return View(vm);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var result = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user); // dang nhap lai
                TempData["success"] = "Your password has been changed.";
                return RedirectToAction(nameof(ChangeInformation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(vm);
        }

        public IActionResult CreateAddress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(Address address)
        {
            if (!ModelState.IsValid) return View(address);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            address.UserId = userId;
            _context.Addresses.Add(address);
            if(await _context.SaveChangesAsync() > 0)
            {
                TempData["success"] = "Create address successfully";
                return RedirectToAction(nameof(ChangeInformation));
            }

            TempData["error"] = "Problem with create address";
            return View(address);
        }

        public async Task<IActionResult> EditAddress(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (address == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Address not found!!!" });

            return View(address);
        }

        [HttpPost]
        public async Task<IActionResult> EditAddress(Address address)
        {
            var addressFromDb = await _context.Addresses.FirstOrDefaultAsync(x=>x.Id == address.Id);
            if(addressFromDb == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Address not found!!!" });

            addressFromDb.FullName = address.FullName;
            addressFromDb.Phone = address.Phone;
            addressFromDb.SpecificAddress = address.SpecificAddress;

            if (await _context.SaveChangesAsync() > 0)
            {
                TempData["success"] = "Edit address successfuly";
                return RedirectToAction(nameof(ChangeInformation));
            }

            TempData["error"] = "Problem with edit address";
            return View(address);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            if (address == null)
                return Json(new { success = false, message = "Address not found" });

            _context.Addresses.Remove(address);
            if (await _context.SaveChangesAsync() > 0)
                return Json(new { success = true, message = "Delete address successfully" });

            return Json(new { success = false, message = "Problem with delete address" });
        }

        public async Task<IActionResult> ManageOrder(OrderParams orderParams)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(orderParams, userId);

            var vm = new OrderVM
            {
                OrderList = orders,
                CurrentSort = orderParams.SortOrder, // sort
                //sort params
                IdSortParm = orderParams.SortOrder == "id" ? "id_desc" : "id",
                TotalSortParm = orderParams.SortOrder == "total" ? "total_desc" : "total",
                CurrentFilter = orderParams.SearchString, // search query
                // query params
                SelectedShipping = orderParams.SelectedShipping,
                SelectedPayment = orderParams.SelectedPayment,
                SelectedStatus = orderParams.SelectedStatus,
                DateStart = orderParams.DateStart,
                DateEnd = orderParams.DateEnd,
                PriceMin = orderParams.PriceMin,
                PriceMax = orderParams.PriceMax,
                //combobox
                ShippingList = await _unitOfWork.ShippingRepository.GetAllContainDeleteAsync()
            };

            return View(vm);
        }

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            return View(order);
        }

        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found!!!" });

            _unitOfWork.OrderRepository.UpdateStatus(orderId, (int)OrderStatus.Canceled);
            if(await _unitOfWork.Complete())
            {
                await _orderHub.Clients.All.SendAsync("CancelOrder", orderId);

                if(order.PaymentStatus == (int)PaymentStatus.Paid) // restore qty when cancel
                {
                    foreach(var item in order.OrderItemList)
                    {
                        if ((item.Size == null && item.Color == null) || (item.Size.IsNullOrEmpty() && item.Color.IsNullOrEmpty())) // no variant
                        {
                            var productNoVariant = await _unitOfWork.ProductRepository.GetProductAsync(item.Name);
                            productNoVariant.Quantity += item.Count;
                        }
                        else // have variant
                        {
                            var product = await _unitOfWork.ProductRepository.GetProductAsync(item.Name, item.Size, item.Color);
                            if (product == null) return Json(new { success = false, message = "Update quantity product fail (product not found)!!!" });

                            var variant = await _unitOfWork.ProductRepository.GetVariantOfProductAsync(product.Id, product.Variants.FirstOrDefault().ColorId,
                                 product.Variants.FirstOrDefault().SizeId);

                            if (variant == null) return Json(new { success = false, message = "Update quantity product fail (variant not found)!!!" });

                            variant.Quantity += item.Count;
                        }
                    }

                    await _unitOfWork.Complete();
                    
                }

                return Json(new { success = true, message = "Order cancel successfully" });
            }

            return Json(new { success = false, message = "Problem with cancel order" });
        }

        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);

            if (order == null) return Json(new { success = false, message = "Order not found!!!" });

            _unitOfWork.OrderRepository.Delete(order);

            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "Delete order successfully" });

            return Json(new { success = false, message = "Problem delete order!!!" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
