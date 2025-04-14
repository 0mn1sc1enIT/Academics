using Academics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Serilog;

namespace Academics.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

		[AllowAnonymous]
		public IActionResult Login()
        {
            _logger.LogInformation("Login page visited");
            return View(new Login {});
        }

        [HttpPost]
        [AllowAnonymous]
		public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login model state");
                return View(login);
            }

            _logger.LogInformation("Attempting to log in user with email: {Email}", login.Email);
            AppUser user = await _userManager.FindByEmailAsync(login.Email);

            if (user != null)
            {

                await _signInManager.SignOutAsync();
                var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, false);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully", login.Email);
                    return RedirectToAction("Index", "Home");
                }
            }

            _logger.LogWarning("Failed login attempt for email: {Email}", login.Email);
            ModelState.AddModelError("", "Неверный email или пароль");
            return View(login);
        }

		[AllowAnonymous]
		public IActionResult Register()
        {
            _logger.LogInformation("Register page visited");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(Register model)
        {
            if (!ModelState.IsValid) {
                _logger.LogWarning("Invalid register model state");
                return View(model);
            }
            var user = new AppUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            _logger.LogInformation("Attempting to register user with email: {Email}", model.Email);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} registered successfully", model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                _logger.LogWarning("Registration error: {Error}", error.Description);
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

		public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                _logger.LogInformation("User {Email} logged out", user.Email);
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
