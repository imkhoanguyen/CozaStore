using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
