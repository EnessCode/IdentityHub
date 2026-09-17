using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityHub.Entities;
using IdentityHub.Models.User;
using IdentityHub.Context;
using IdentityHub.Service.EmailService;
using System.Threading.Tasks;
using System.Security.Claims;

namespace IdentityHub.Controllers
{
	public class LoginController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IEmailService _emailService;

		public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailService = emailService;
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

			var user = await _userManager.FindByNameAsync(model.Username);

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

		[HttpPost]
		public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
		{
			var redirectUrl = Url.Action("ExternalLoginCallBack", "Login", new { returnUrl });
			var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

			return Challenge(properties, provider);
		}

		[HttpGet, HttpPost]
		public async Task<IActionResult> ExternalLoginCallBack(string returnUrl = null, string remoteError = null)
		{
			returnUrl ??= Url.Content("~/");
			if (remoteError != null)
			{
				ModelState.AddModelError("", $"External Provider Error:{remoteError}");
				return RedirectToAction("Index");
			}

			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null)
			{
				return RedirectToAction("Index");
			}

			var result = await _signInManager.ExternalLoginSignInAsync
				(info.LoginProvider, info.ProviderKey, isPersistent: false);

			if (result.Succeeded)
			{
				return RedirectToAction("Inbox", "Message");
			}
			else
			{
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);

				var user = await _userManager.FindByEmailAsync(email);

				if (user == null)
				{
					var givenName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? info.Principal.FindFirstValue(ClaimTypes.Name) ?? email.Split('@')[0];
					var surname = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? "Bilinmeyen";

					user = new AppUser()
					{
						UserName = email,
						Email = email,
						Name = givenName,
						Surname = surname,
						EmailConfirmed = true
					};

					var identityResult = await _userManager.CreateAsync(user);
					if (!identityResult.Succeeded)
					{
						return RedirectToAction("Index");
					}
				}

				await _userManager.AddLoginAsync(user, info);
				await _signInManager.SignInAsync(user, isPersistent: false);

				return RedirectToAction("Inbox", "Message");
			}
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByEmailAsync(model.Email);

			if (user == null)
			{
				TempData["Error"] = "Sistemde bu e-posta adresiyle eşleşen bir hesap bulunamadı.";
				return View(model);
			}

			var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var resetLink = Url.Action("ResetPassword", "Login", new
			{
				userId = user.Id,
				token = token
			}, HttpContext.Request.Scheme);

			await _emailService.SendPasswordResetEmailAsync(model.Email, resetLink);

			TempData["Success"] = "Şifre sıfırlama bağlantısı e-posta adresinize başarıyla gönderildi.";

			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult ResetPassword(string userId, string token)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
			{
				TempData["Error"] = "Geçersiz veya süresi dolmuş şifre sıfırlama bağlantısı.";
				return RedirectToAction("Index");
			}

			var model = new ResetPasswordViewModel
			{
				UserId = userId,
				Token = token
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var user = await _userManager.FindByIdAsync(model.UserId);
			if (user == null)
			{
				TempData["Error"] = "Kullanıcı bulunamadı.";
				return View(model);
			}

			var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
			if (result.Succeeded)
			{
				TempData["Success"] = "Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz.";
				return RedirectToAction("Index");
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}

			return View(model);
		}
	}
}
