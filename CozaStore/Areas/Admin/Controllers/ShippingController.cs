using CozaStore.Interfaces;
using CozaStore.Models;
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
        public async Task<IActionResult> Index(string searchString, int page)
        {
            var shippingMethods = await _unitOfWork.ShippingRepository.GetAllShippingMethodsAsync(searchString, page);

            return View(shippingMethods);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShippingMethod shippingMethod)
        {
            if (!ModelState.IsValid) return View();

            _unitOfWork.ShippingRepository.Add(shippingMethod);
            if(await _unitOfWork.Complete())
            {
                TempData["success"] = "Shipping method has been created successfully";
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("GetBadRequest", "Buggy", new { area = "", message = "Problem create shipping method" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingRepository.GetAsync(id);
            if(shippingMethod == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Shipping method not found" });

            return View(shippingMethod);
        }

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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var shippingMethod = await _unitOfWork.ShippingRepository.GetAsync(id);
            if(shippingMethod == null)
            {
                TempData["error"] = "Shipping method not found";
                return NoContent();
            }

            _unitOfWork.ShippingRepository.Delete(shippingMethod);

            if (await _unitOfWork.Complete())
            {
                TempData["success"] = "Delete shipping method successfully";
                return NoContent();
            }
            TempData["error"] = "Problem delete shipping method";
            return NoContent();
        }
    }
}
