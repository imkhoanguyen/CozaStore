using CozaStore.Models;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index(string searchString, int page)
        {
            ViewData["searchString"] = searchString;
            var colors = await _unitOfWork.ColorRepository.GetAllColorsAsync(searchString, page);
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var color = await _unitOfWork.ColorRepository.GetColorAsync(id);
            if (color != null)
            {
                bool isDelte = color.IsDelete;
                _unitOfWork.ColorRepository.ToggleDelete(color);

                if (await _unitOfWork.Complete())
                {
                    if (isDelte)
                    {
                        TempData["success"] = "The color has been reverted successfully.";
                        return Json(new { message = "success" });
                    }
                    else
                    {
                        TempData["success"] = "The color has been deleted successfully.";
                        return NoContent();
                    }

                }
            }
            TempData["error"] = "Color not found!!!";
            return NoContent();
        }
    }
}
