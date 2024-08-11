using CozaStore.Models;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index(int page)
        {
            var sizes = await _unitOfWork.SizeRepository.GetAllSizesAsync(page);
            return View(sizes);
        }


        public IActionResult Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Edit(int id)
        {
            var size = await _unitOfWork.SizeRepository.GetSizeAsync(id);
            if (size != null) return View(size);
            return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Size not found" });

        }

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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var size = await _unitOfWork.SizeRepository.GetSizeAsync(id);
            if (size != null)
            {
                bool isDelte = size.IsDelete;
                _unitOfWork.SizeRepository.ToggleDelete(size);

                if (await _unitOfWork.Complete())
                {
                    if (isDelte)
                    {
                        TempData["success"] = "The size has been reverted successfully.";
                        return NoContent();
                    }
                    else
                    {
                        TempData["success"] = "The size has been deleted successfully.";
                        return NoContent();
                    }
                }
            }
            TempData["error"] = "Size not found!!!";
            return NoContent();
        }
    }
}
