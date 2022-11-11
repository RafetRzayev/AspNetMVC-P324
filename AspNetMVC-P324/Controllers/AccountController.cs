using AspNetMVC_P324.Models.IdentityModels;
using AspNetMVC_P324.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic;
using NuGet.Protocol;

namespace AspNetMVC_P324.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View();

            var existUser = await _userManager.FindByNameAsync(model.Username);

            if (existUser != null)
            {
                ModelState.AddModelError("", "Username tekrarlana bilmez");
                return View();
            }

            var user = new User
            {
                Fullname = model.Fullname,
                UserName = model.Username,
                Email = model.Email
            };

            //var role = await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });

            var result = await _userManager.CreateAsync(user, model.Password);

            //await _userManager.AddToRoleAsync(user, "Admin");

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _signInManager.SignInAsync(user, false);

            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var existUser =await  _userManager.FindByNameAsync(model.Username);

                if (existUser == null)
                {
                    ModelState.AddModelError("", "Username isnot correct");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(existUser, model.Password, false, true);

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Email tesdiqlenmelidir");
                    return View();
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "This user locked out");
                    return View();
                }

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Invalid credentials");
                    return View();
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction(nameof(Login));

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var existUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (existUser == null)
                return BadRequest();

            var result = await _userManager.ChangePasswordAsync(existUser, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View();
            }

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
