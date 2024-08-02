using CozaStore.Entities;
using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var categories = _unitOfWork.CategoryRepository.GetAllCategories();
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
                
                if(await _unitOfWork.Complete()) return RedirectToAction("Index" , "Dashboard");
            }
            return View();
        }
    }
}
