using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ShippingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShippingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = ClaimStore.AccessShipping)]
        public async Task<IActionResult> Index(ShippingParams shippingParams)
        {
            ViewData["searchString"] = shippingParams.SearchString;
            ViewData["pageSize"] = (int)shippingParams.PageSize;
            var shippingMethods = await _unitOfWork.ShippingRepository.GetAllShippingMethodsAsync(shippingParams);

            return View(shippingMethods);
        }

        [Authorize(Policy = ClaimStore.Shipping_Create)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = ClaimStore.Shipping_Create)]
        [HttpPost]
        public async Task<IActionResult> Create(ShippingMethod shippingMethod)
        {
            if (!ModelState.IsValid) return View();

            _unitOfWork.ShippingRepository.Add(shippingMethod);
            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "Shipping method has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Problem create shipping method" });
        }


        [Authorize(Policy = ClaimStore.Shipping_Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingRepository.GetAsync(id);
            if (shippingMethod == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Shipping method not found" });

            return View(shippingMethod);
        }

        [Authorize(Policy = ClaimStore.Shipping_Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(ShippingMethod shippingMethod)
        {
            if (!ModelState.IsValid || shippingMethod.Id < 1) return View();
            _unitOfWork.ShippingRepository.Update(shippingMethod);
            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "Shipping method has been edited successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Problem edit shipping method" });
        }

        [Authorize(Policy = ClaimStore.Shipping_Delete)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingRepository.GetAsync(id);
            if (shippingMethod == null)
                return Json(new { success = false, message = "Shipping method not found!!!" });

            _unitOfWork.ShippingRepository.Delete(shippingMethod);

            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "Delete shipping method successfully" });

            return Json(new { success = false, message = "Problem delete shipping method" });
        }
    }
}
