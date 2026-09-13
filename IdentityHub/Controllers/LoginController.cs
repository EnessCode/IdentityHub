using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityHub.Entities;
using IdentityHub.Models.User;
using IdentityHub.Context;

namespace IdentityHub.Controllers
{
	public class LoginController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IdentityContext _context;

		public LoginController(SignInManager<AppUser> signInManager, IdentityContext context)
		{
			_signInManager = signInManager;
			_context = context;
		}

		[HttpGet]
		public IActionResult Index() 
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(LoginViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = _context.Users.FirstOrDefault(x => x.UserName == model.Username);

			if (user == null)
			{
				ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
				return View(model);
			}

			if (!user.EmailConfirmed)
			{
				ModelState.AddModelError("", "Lütfen giriş yapmadan önce e-posta adresinizi doğrulayın.");
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, true, true);

			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Profile");
			}
			else
			{
				ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı.");
			}

			return View(model);
		}
	}
}
