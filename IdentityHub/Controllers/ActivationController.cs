using IdentityHub.Context;
using IdentityHub.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.Controllers
{
	public class ActivationController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public ActivationController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpGet]
		public IActionResult Index(string email)
		{
			ViewBag.Email = email; 
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(string email, int userCode)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
			{
				ModelState.AddModelError("", "Kullanıcı bulunamadı.");
				return View();
			}

			if (user.ActivationCode == userCode)
			{
				user.EmailConfirmed = true;
				await _userManager.UpdateAsync(user);

				return RedirectToAction("Index", "Login");
			}

			ModelState.AddModelError("", "Hatalı doğrulama kodu girdiniz.");
			ViewBag.Email = email;
			return View();
		}
	}
}