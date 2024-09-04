using CloudinaryDotNet.Actions;
using CozaStore.Data;
using CozaStore.Helpers;
using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
        public async Task<IActionResult> Index(UserParams userParams)
        {
            ViewData["searchString"] = userParams.SearchString;
            ViewData["pageSize"] = (int)userParams.PageSize;

            var query = _context.AppUser.AsQueryable();

            if(!userParams.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x=>x.FullName.ToLower().Contains(userParams.SearchString.ToLower()) 
                || x.PhoneNumber.Contains(userParams.SearchString));
            }

            var userRole = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            foreach(var user in query.ToList())
            {
                var user_role = userRole.FirstOrDefault(u=>u.UserId == user.Id);
                if (user_role == null) user.Role = null;
                else user.Role = roles.FirstOrDefault(u=>u.Id == user_role.RoleId).Name;
            }

            var users = await PagedList<AppUser>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
            return View(users);
        }

        public async Task<IActionResult> ManageRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "User not found!!!" });

            //role name
            List<string> exsitingUserRoles = await _userManager.GetRolesAsync(user) as List<string>;
            var roleName = exsitingUserRoles.FirstOrDefault();

            if (roleName == null)
            {
                UserRoleVM vm = new UserRoleVM
                {
                    RoleList = await _context.AppRole.ToListAsync(),
                    userId = userId,
                };
                return View(vm);
            }
            else
            {
                UserRoleVM vm = new UserRoleVM
                {
                    RoleList = await _context.AppRole.ToListAsync(),
                    userId = userId,
                    SelectedRoleName = roleName
                };
                return View(vm);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ManageRole(UserRoleVM vm)
        {
            var user = await _userManager.FindByIdAsync(vm.userId);
            if (user == null)
                return RedirectToAction("GetNotFound", "Buggy", new { area = "", message = "User not found!!!" });

            var oldUserRoles = await _userManager.GetRolesAsync(user);
            var oldRoleName = oldUserRoles.FirstOrDefault();
            var result = await _userManager.RemoveFromRoleAsync(user, oldRoleName);

            if(!result.Succeeded)
            {
                TempData["error"] = "Error with removing role";
                return RedirectToAction(nameof(Index));
            }

            result = await _userManager.AddToRoleAsync(user, vm.SelectedRoleName);
            if(result.Succeeded)
            {
                TempData["success"] = "Role assigned successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Error while adding role";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleLockout(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return Json(new { success = false, message = "User not found" });
            
            if(user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.Now) // was locked
            {
                // unlock
                await _userManager.SetLockoutEndDateAsync(user, null);
                await _userManager.ResetAccessFailedCountAsync(user); // reset failed login
                return Json(new { success = true, message = "Unlock user successfully", action ="unlock", id = user.Id });
            } else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                return Json(new { success = true, message = "Lock user successfully", action = "lock", id = user.Id });
            }
        }
    }
}
