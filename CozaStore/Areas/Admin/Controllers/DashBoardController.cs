using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DashboardController : Controller
    {
        [Route("/admin")]
        [Route("/admin/dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
