using Azure;
using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class RoleController : Controller
    {
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(DataContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(string searchString, int page)
        {
            if (page < 1) page = 1;

            var query = _context.AppRole.AsQueryable();
            if(!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x=>x.Name!.ToLower().Contains(searchString.ToLower()));
            }

            var role = await PagedList<AppRole>.CreateAsync(query, page, 10);
            return View(role);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleCreateVM vm)
        {
            if (!ModelState.IsValid) return View();

            var appRole = new AppRole
            {
                Name = vm.Name,
                Description = vm.Description
            };

            var result = await _roleManager.CreateAsync(appRole);

            if (result.Succeeded)
            {
                TempData["success"] = "Role created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _context.AppRole.FindAsync(id);
            if (role == null)
                return RedirectToAction("GeNotFound", "Buggy", new { area = "", message = "Role not found!!!" });

            var vm = new RoleCreateVM
            {
                Id = role.Id,
                Name = role.Name!,
                Description = role.Description,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleCreateVM vm)
        {
            if (!ModelState.IsValid || vm.Id.IsNullOrEmpty()) return View();

            var roleFromDb = _context.AppRole.Find(vm.Id);
            if (roleFromDb == null) return RedirectToAction("GeNotFound", "Buggy", new { area = "", message = "Role not found!!!" });

            roleFromDb.Name = vm.Name;
            roleFromDb.Description = vm.Description;
            roleFromDb.NormalizedName = vm.Name.ToUpper();

            var result = await _roleManager.UpdateAsync(roleFromDb);
            if (result.Succeeded)
            {
                TempData["success"] = "Role updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var role = _context.AppRole.Find(id);
            if(role == null) 
                return RedirectToAction("GeNotFound", "Buggy", new { area = "", message = "Role not found!!!" });

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["success"] = "Role deleted successfully!";
                return NoContent();
            }
            else
            {
                string errorStr = "";
                foreach (var error in result.Errors)
                {
                    errorStr += error;
                }
                TempData["error"] = errorStr;
                return NoContent();
            }
        }
    }
}
