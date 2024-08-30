using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
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
        public async Task<IActionResult> Index(OrderParams orderParams)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(orderParams);

            var vm = new OrderVM
            {
                OrderList = orders,
                CurrentFilter = orderParams.SearchString, // search query
                SelectedShipping = orderParams.SelectedShipping,
                SelectedPayment = orderParams.SelectedPayment,
                SelectedStatus = orderParams.SelectedStatus,
                DateStart = orderParams.DateStart,
                DateEnd = orderParams.DateEnd,
                PriceMin = orderParams.PriceMin,
                PriceMax = orderParams.PriceMax,
                //combobox
                ShippingList = await _unitOfWork.ShippingRepository.GetAllContainDeleteAsync()
            };

            

            return View(vm);
        }
    }
}
