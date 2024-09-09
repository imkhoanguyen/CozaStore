using CozaStore.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardController : Controller
    {
        [Route("/admin")]
        [Route("/admin/dashboard")]
        [Authorize(Policy = ClaimStore.AccessDashboard)]
        public IActionResult Index()
        {
            return View();
        }
    }
}
