using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityHub.Entities;
using IdentityHub.Models.User;

namespace IdentityHub.Controllers
{
	public class RegisterController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public RegisterController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _userManager.CreateAsync(new AppUser
			{
				Name = model.Name,
				Surname = model.Surname,
				Email = model.Email,
				UserName = model.Username
			}, model.Password);

			if (result.Succeeded)
			{
				return RedirectToAction("Login", "Login");
			}
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}

			return View();
		}
	}
}
