using CozaStore.Models;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CozaStore.Helpers;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ColorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ColorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(ColorParams colorParams)
        {
            ViewData["searchString"] = colorParams.SearchString;
            ViewData["pageSize"] = (int)colorParams.PageSize;
            var colors = await _unitOfWork.ColorRepository.GetAllColorsAsync(colorParams);
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Color color)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ColorRepository.AddColor(color);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The color has been created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetColorAsync(id);
            if (color != null) return View(color);
            return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Color not found" });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Color color)
        {
            if (ModelState.IsValid && color.Id > 0)
            {
                _unitOfWork.ColorRepository.UpdateColor(color);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The color has been edited successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetColorAsync(id);
            if (color != null)
            {
                _unitOfWork.ColorRepository.Delete(color);

                if (await _unitOfWork.Complete())
                    return Json(new { success = true, message = "The color has been deleted successfully." });
            }
            return Json(new { success = false, message = "Color not found!!!" });
        }
    }
}
