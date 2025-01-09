using CozaStore.Data;
using CozaStore.Data.Enum;
using CozaStore.Interfaces;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork _unit;
        private readonly UserManager<AppUser> _userManager;
        public DashboardController(IUnitOfWork unit, UserManager<AppUser> userManager)
        {
            _unit = unit;
            _userManager = userManager;
        }

        [Route("/admin")]
        [Route("/admin/dashboard")]
        [Authorize(Policy = ClaimStore.AccessDashboard)]
        public async Task<IActionResult> Index()
        {
            
            var orders = await _unit.OrderRepository.GetAllAsync();

            var statistics = StatisticRevenueInYear(orders);

            var totalPriceList = statistics.Select(s => s.TotalPrice).ToList();
            var totalOrdersList = statistics.Select(s => s.TotalOrders).ToList();

            var listOrderConfirmed = orders.Where(o => o.OrderStatus != (int)OrderStatus.Canceled);

            var vm = new DashboardVM
            {
                TotalPriceList = totalPriceList,
                TotalOrderList = totalOrdersList,
                TotalPriceToday = GetTotalPriceToday(orders),
                TotalUser = GetTotalUser(),
                TotalProduct = await GetTotalProduct(),
                TotalOrder = listOrderConfirmed.Count(),
                TopProductList = GetTopProduct(listOrderConfirmed).OrderByDescending(x => x.Count),
            };


            return View(vm);
        }

        #region

        private IEnumerable<(decimal TotalPrice, int TotalOrders)> StatisticRevenueInYear(IEnumerable<Order> orders)
        {
            var currentYear = DateTime.Now.Year;
            var currentYearOrders = orders.Where(o => o.OrderDate.Year == currentYear);

            var monthlyOrderTotals = currentYearOrders
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalPrice = g.Sum(o => o.GetTotal()),
                    TotalOrders = g.Count()
                })
                .ToList();

            var totalOrdersPerMonth = new (decimal TotalPrice, int TotalOrders)[12];
            foreach (var monthData in monthlyOrderTotals)
            {
                totalOrdersPerMonth[monthData.Month - 1] = (monthData.TotalPrice, monthData.TotalOrders);
            }

            return totalOrdersPerMonth;
        }

        private decimal GetTotalPriceToday(IEnumerable<Order> orders)
        {
            var today = DateTime.Today;
            var list = orders.Where(o => o.OrderDate.Date == today);

            decimal totalPrice = list.Sum(o => o.GetTotal());
            return totalPrice;
        }

        private int GetTotalUser()
        {
            var total = _userManager.Users.ToList().Count();
            return total;
        }

        private async Task<int> GetTotalProduct()
        {
            var total = await _unit.ProductRepository.GetTotalProduct();
            return total;
        }

        private IEnumerable<TopProductVM> GetTopProduct(IEnumerable<Order> orders)
        {
            var topProductOrdered = orders
                .SelectMany(o => o.OrderItemList) 
                .GroupBy(item => item.Name) 
                .Select(g => new
                {
                    ProductName = g.First().Name,
                    OrderCount = g.Count(),
                    ProductImg = g.First().Url
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .Select(x => new TopProductVM
                {
                    Name = x.ProductName,
                    ImgUrl = x.ProductImg,
                    Count = x.OrderCount
                })
                .ToList();

            return topProductOrdered;
        }



        #endregion
    }
}
