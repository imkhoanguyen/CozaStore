using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly DataContext _context;
        public UserController(UserManager<AppUser> userManager, DataContext context)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchString, int page)
        {
            if (page < 1) page = 1;
            var users = await PagedList<AppUser>.CreateAsync(_context.AppUser.AsQueryable(), page, 10);
            return View(users);
        }
    }
}
