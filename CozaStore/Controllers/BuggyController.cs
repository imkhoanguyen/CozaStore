using CozaStore.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Controllers
{
    public class BuggyController : Controller
    {
        public IActionResult ServerError(int statusCode, string message, string details)
        {
            var exception = new AppException(statusCode, message, details);
            return View(exception);
        }

        public IActionResult GetNotFound(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        public IActionResult GetBadRequest(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }
    }
}
