using CozaStore.Models;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CozaStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager; 
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM vm)
        {
            if(!ModelState.IsValid) return View();

            var appUser = vm.UserName.Contains("@")
             ? await _userManager.FindByEmailAsync(vm.UserName)
             : await _userManager.FindByNameAsync(vm.UserName);

            var result = await _signInManager.PasswordSignInAsync(appUser, vm.Password, vm.RememberMe, lockoutOnFailure: false);

            if(result.Succeeded)
            {
                TempData["success"] = "Login success!!!";
                return RedirectToAction("Index", "Home");
            } else
            {
                ModelState.AddModelError("Login", "Failed to login");
                return View();
            }
        }

        public IActionResult Register()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vm)
        {
            if (!ModelState.IsValid) return View();

            var user = new AppUser
            {
                UserName = vm.UserName,
                FullName = vm.FullName,
                PhoneNumber = vm.PhoneNumber,
                Email = vm.Email,
                Gender = vm.Gender,
                Image = "/img/avatar.png",
            };

            var result =  await _userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                TempData["success"] = "Register successfully!";
                return RedirectToAction("Login", "Account");
            }
            AddErrors(result);
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
