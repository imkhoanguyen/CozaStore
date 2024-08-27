using CozaStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string searchString, int page)
        {
            var order = await _unitOfWork.OrderRepository.GetAllAsync(searchString, page);
            return View(order);
        }
    }
}
