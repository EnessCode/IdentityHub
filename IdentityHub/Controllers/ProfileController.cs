using IdentityHub.Entities;
using IdentityHub.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.Controllers
{
	public class ProfileController : Controller
	{
		private readonly UserManager<AppUser> _userManager;

		public ProfileController(UserManager<AppUser> userManager)
		{
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			UserEditViewModel userEditViewModel = new UserEditViewModel()
			{
				Username = user.UserName,
				Name = user.Name,
				Surname = user.Surname,
				City = user.City,
				ImageUrl = user.ImageUrl,
				PhoneNumber = user.PhoneNumber,
				Email = user.Email
			};
			return View(userEditViewModel);
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(UserEditViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByNameAsync(User.Identity.Name);

				user.Name = model.Name;
				user.Surname = model.Surname;
				user.City = model.City;
				user.ImageUrl = model.ImageUrl;
				user.PhoneNumber = model.PhoneNumber;
				user.Email = model.Email;

				if (!string.IsNullOrEmpty(model.Password))
				{
					user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
				}

				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					return RedirectToAction("EditProfile", "Profile");
				}

				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
			}
			return View(model);
		}
	}
}
