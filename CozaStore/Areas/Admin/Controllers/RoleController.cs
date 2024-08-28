using Azure;
using CloudinaryDotNet.Actions;
using CozaStore.Data;
using CozaStore.DTOs;
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
    public class RoleController : Controller
    {
        private readonly DataContext _context;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(DataContext context, RoleManager<AppRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index(RoleParams roleParams)
        {
            ViewData["searchString"] = roleParams.SearchString;
            ViewData["pageSize"] = (int)roleParams.PageSize;
            var query = _context.AppRole.AsQueryable();
            if (!roleParams.SearchString.IsNullOrEmpty())
            {
                query = query.Where(x => x.Name.ToLower().Contains(roleParams.SearchString.ToLower()));
            }

            var role = await PagedList<AppRole>.CreateAsync(query, roleParams.PageNumber, roleParams.PageSize);
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
            if (role == null)
                return Json(new { success = false, message = "Role not found!" });

            var usersWithRole = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .ToListAsync();

            if (usersWithRole.Any())
                return Json(new { success = false, message = "Cannot delete the role as there are users associated with it." });

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Json(new { success = true, message = "Role deleted successfully!" });
            else
            {
                string errorStr = "";
                foreach (var error in result.Errors)
                {
                    errorStr += error;
                }
                return Json(new { success = false, message = "errorStr" });
            }
        }

        public async Task<IActionResult> ManagePermission(string roleId)
        {
            var role = await _context.AppRole.FindAsync(roleId);
            var roleClaimList = await _context.RoleClaims.Where(rc => rc.RoleId == roleId).ToListAsync();

            var perrmissionGroup = ClaimStore.AllPermissionGroups
                 .Select(group => new PermissionGroupDto
                 {
                     GroupName = group.GroupName,
                     Permissions = group.Permissions.Select(permissionItem => new PermissionItemDto
                     {
                         Name = permissionItem.Name,
                         ClaimValue = permissionItem.ClaimValue,
                         IsSelected = roleClaimList.Any(rc => rc.ClaimValue == permissionItem.ClaimValue)
                     }).ToList()
                 }).ToList();

            var vm = new RolePermissionVM
            {
                AppRole = role,
                PermissionGroups = perrmissionGroup,
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ManagePermission(RolePermissionVM vm)
        {
            if(vm.AppRole.Name == "Admin")
            {
                TempData["error"] = "Cant not edit permission admin role";
                return RedirectToAction(nameof(Index));
            }
            var roleClaimList = await _context.RoleClaims.Where(rc => rc.RoleId == vm.AppRole.Id).ToListAsync();
            var selectedClaimValueList = vm.SelectedClaimValueList;

            var claimValueToAdd = selectedClaimValueList.Except(roleClaimList.Select(rc => rc.ClaimValue));
            var claimValueToDelete = roleClaimList.Select(rc => rc.ClaimValue).Except(selectedClaimValueList);

            var listClaimToAdd = claimValueToAdd.Select(claimValue => new IdentityRoleClaim<string>
            {
                RoleId = vm.AppRole.Id,
                ClaimType = "Permission",
                ClaimValue = claimValue
            }).ToList();

            if (listClaimToAdd.Any())
                await _context.RoleClaims.AddRangeAsync(listClaimToAdd);

            // cái này là tạo mới nên nó sẽ tạo ra một instance mới của identityRoleClaim nó có id hoặc giá trị tạm thời (id mới) => ko tìm được id để xóa => lỗi
            //var listClaimToDelete = claimValueToDelete.Select(claimValue => new IdentityRoleClaim<string>
            //{
            //    RoleId = vm.AppRole.Id,
            //    ClaimType = "Permission",
            //    ClaimValue = claimValue
            //}).ToList();

            var listClaimToDelete = roleClaimList
            .Where(rc => claimValueToDelete.Contains(rc.ClaimValue))
            .ToList();

            if (listClaimToDelete.Any())
                _context.RoleClaims.RemoveRange(listClaimToDelete);

            if (await _context.SaveChangesAsync() > 0)
            {
                TempData["success"] = "Update premission success";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Update permission error";
            return RedirectToAction(nameof(Index));
        }
    }
}
