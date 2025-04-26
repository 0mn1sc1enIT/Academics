using Academics.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Academics.Admin.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public UserController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			var users = _userManager.Users.ToList();
			return View(users);
		}

		public async Task<IActionResult> Edit(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(AppUser model)
		{
			var user = await _userManager.FindByIdAsync(model.Id);
			if (user == null) return NotFound();

			user.Email = model.Email;
			user.UserName = model.UserName;
			user.PhoneNumber = model.PhoneNumber;

			await _userManager.UpdateAsync(user);

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user != null)
			{
				await _userManager.DeleteAsync(user);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
