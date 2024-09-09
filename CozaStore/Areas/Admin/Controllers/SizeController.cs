using CozaStore.Models;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CozaStore.Helpers;
using Microsoft.AspNetCore.Authorization;
using CozaStore.Data;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public SizeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize(Policy = ClaimStore.AccessSize)]
        public async Task<IActionResult> Index(SizeParams sizeParams)
        {
            ViewData["searchString"] = sizeParams.SearchString;
            ViewData["pageSize"] = (int)sizeParams.PageSize;
            var sizes = await _unitOfWork.SizeRepository.GetAllSizesAsync(sizeParams);
            return View(sizes);
        }

        [Authorize(Policy = ClaimStore.Size_Create)]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = ClaimStore.Size_Create)]
        [HttpPost]
        public async Task<IActionResult> Create(Size size)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SizeRepository.AddSize(size);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The size has been created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [Authorize(Policy = ClaimStore.Size_Edit)]
        public async Task<IActionResult> Edit(int id)
        {
            var size = await _unitOfWork.SizeRepository.GetSizeAsync(id);
            if (size != null) return View(size);
            return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Size not found" });

        }

        [Authorize(Policy = ClaimStore.Size_Edit)]
        [HttpPost]
        public async Task<IActionResult> Edit(Size size)
        {
            if (ModelState.IsValid && size.Id > 0)
            {
                _unitOfWork.SizeRepository.UpdateSize(size);
                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The size has been edited successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [Authorize(Policy = ClaimStore.Size_Delete)]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var size = await _unitOfWork.SizeRepository.GetSizeAsync(id);
            if (size != null)
            {
                _unitOfWork.SizeRepository.Delete(size);

                if (await _unitOfWork.Complete())
                    return Json(new { success = true, message = "The size has been deleted successfully." });
            }
            return Json(new { success = false, message = "Size not found!!!" });
        }
    }
}
