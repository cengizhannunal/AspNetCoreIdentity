using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreIdentity.Identity;
using AspNetCoreIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;

        public AccountController(UserManager<AppIdentityUser> userManager, SignInManager<AppIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Route("administrator/register")]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {

            };
            return View(model);
        }

        [HttpPost]
        [Route("administrator/register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new AppIdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var validateItem in result.Errors)
                    ModelState.AddModelError("", validateItem.Description);

                return View(model);
            }

            return Redirect("~/administrator/login");
        }




        [Route("administrator/login")]
        public IActionResult Login()
        {
            var model = new LoginViewModel
            {

            };
            return View(model);
        }

        [HttpPost]
        [Route("administrator/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
            if (result.Succeeded)
            {
                return Redirect("~/administrator/dashboard");
            }

            return View(model);
        }

        [Route("administrator/log-out")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/administrator/login");
        }

        [Route("administrator/access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}