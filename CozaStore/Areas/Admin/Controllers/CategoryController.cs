using Azure;
using CozaStore.Entities;
using CozaStore.Helpers;
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

        [Route("/Admin/Category/SubCategory")]
        [HttpGet]
        public async Task<IActionResult> SubCategory(int categoryId, int page)
        {
            ViewData["CategoryId"] = categoryId;
            var subCategories = await _unitOfWork.SubCategoryRepository.GetAllSubCategoriesAsync(categoryId, page);
            SubCategoryVM vm = new SubCategoryVM()
            {
                SubCategories = subCategories,
                CategoryId = categoryId
            };
            return View(vm);
        }

        [Route("/Admin/Category/SubCategory/Create")]
        public IActionResult SubCategoryCreate(int categoryId)
        {
            SubCategoryVM vm = new SubCategoryVM()
            {
                CategoryId = categoryId
            };
            return View(vm);
        }

        [HttpPost]
        [Route("/Admin/Category/SubCategory/Create")]
        public async Task<IActionResult> SubCategoryCreate(SubCategoryVM vm)
        {

            if (ModelState.IsValid && vm.SubCategory != null)
            {
                vm.SubCategory.CategoryId = vm.CategoryId;
                _unitOfWork.SubCategoryRepository.AddSubCategory(vm.SubCategory);

                if (await _unitOfWork.Complete())
                    return RedirectToAction(nameof(SubCategory), new { categoryId = vm.SubCategory.CategoryId });
            }

            return View();
        }


        [Route("/Admin/Category/SubCategory/Update")]
        public async Task<IActionResult> SubCategoryUpdate(int categoryId, int subCategoryId)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetSubCategoryAsync(subCategoryId);
            if (subCategory != null)
            {
                SubCategoryVM vm = new SubCategoryVM()
                {
                    SubCategory = subCategory,
                    CategoryId = subCategory.CategoryId
                };
                return View(vm);
            }
            return RedirectToAction(nameof(SubCategory), new { categoryId = categoryId });
        }

        [HttpPost]
        [Route("/Admin/Category/SubCategory/Update")]
        public async Task<IActionResult> SubCategoryUpdate(SubCategoryVM vm)
        {
            if (ModelState.IsValid && vm.SubCategory != null)
            {
                vm.SubCategory.CategoryId = vm.CategoryId;
                _unitOfWork.SubCategoryRepository.UpdateSubCategory(vm.SubCategory);
                if (await _unitOfWork.Complete())
                    return RedirectToAction(nameof(SubCategory), new { categoryId = vm.SubCategory.CategoryId });
            }
            return View();
        }

        [HttpPost]
        [Route("/Admin/Category/SubCategory/Delete")]
        public async Task<IActionResult> SubCategoryDelete(int subCategoryId)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetSubCategoryAsync(subCategoryId);
            if (subCategory != null)
            {
                _unitOfWork.SubCategoryRepository.RemoveSubCategory(subCategory);

                if (await _unitOfWork.Complete())
                    return Json(new { message = "Xóa thành công" });

            }
            return Json(new { message = "Xóa thất bại" });
        }
    }
}
