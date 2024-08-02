using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        [Route("/admin")]
        [Route("/admin/dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
