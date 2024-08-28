using CozaStore.Models;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CozaStore.Data;
using CozaStore.Helpers;

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

        public async Task<IActionResult> Index(CategoryParams categoryParams)
        {
            ViewData["searchString"] = categoryParams.SearchString;
            ViewData["pageSize"] = (int)categoryParams.PageSize;
            var categories = await _unitOfWork.CategoryRepository.GetAllCategoriesAsync(categoryParams);
            return View(categories);
        }

        //[Authorize(Policy = ClaimStore.Category_Create)]
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

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The category has been created successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public async Task<IActionResult> Edit(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(categoryId);
            if (category == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "Category not found" });
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (category != null && ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.UpdateCategory(category);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The category has been edited successfully.";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryAsync(categoryId);
            if (category != null)
            {
                _unitOfWork.CategoryRepository.DeleteCategory(category);

                if (await _unitOfWork.Complete())
                {
                    TempData["success"] = "The category has been deleted successfully.";
                    return NoContent();
                }
            }
            TempData["error"] = "Category not found!!!";
            return NoContent();
        }

    }
}
