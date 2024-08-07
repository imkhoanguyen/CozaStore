using CozaStore.Models;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string searchString, int page)
        {
            ViewData["SearchString"] = searchString;
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync(searchString, page);
            return View(categories);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.AddCategory(category);

                if (await _unitOfWork.Complete()) return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public async Task<IActionResult> Update(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(categoryId);
            if(category != null)
            {
                return View(category);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            if(category != null && ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.UpdateCategory(category);

                if (await _unitOfWork.Complete()) return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(categoryId);
            if(category != null)
            {
                _unitOfWork.CategoryRepository.DeleteCategory(category);

                if(await _unitOfWork.Complete())
                    return Json(new { message = "Xóa thành công" });
            }
            return Json(new { message = "Xóa thất bại" });
        }
        
    }
}
