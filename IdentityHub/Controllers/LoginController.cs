using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityHub.Entities;
using IdentityHub.Models.User;

namespace IdentityHub.Controllers
{
	public class LoginController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;

		public LoginController(SignInManager<AppUser> signInManager)
		{
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
			}
			return View(model);
		}
	}
}