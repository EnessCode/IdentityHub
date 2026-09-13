using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentityHub.Entities;
using IdentityHub.Models.User;
using MimeKit;
using MailKit.Net.Smtp;
using IdentityHub.Service.EmailService;

namespace IdentityHub.Controllers
{
	public class RegisterController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IEmailService _emailService;

		public RegisterController(UserManager<AppUser> userManager, IEmailService emailService)
		{
			_userManager = userManager;
			_emailService = emailService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(RegisterViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			int code = new Random().Next(100000, 999999);

			var result = await _userManager.CreateAsync(new AppUser
			{
				Name = model.Name,
				Surname = model.Surname,
				Email = model.Email,
				UserName = model.Username,
				ActivationCode = code
			}, model.Password);

			if (result.Succeeded)
			{
				await _emailService.SendActivationEmailAsync(model.Email, code);
				return RedirectToAction("Index", "Activation", new { email = model.Email });
			}

			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
			return View();
		}
	}
}
