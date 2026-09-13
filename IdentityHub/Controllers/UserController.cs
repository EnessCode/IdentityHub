using IdentityHub.Entities;
using IdentityHub.Models.Role;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityHub.Controllers
{
	public class UserController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.ToListAsync();
			return View(users);
		}

		[HttpGet]
		public async Task<IActionResult> AssignRole(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound(); 

			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			TempData["UserId"] = user.Id;
			var roles = await _roleManager.Roles.ToListAsync();
			var userRoles = await _userManager.GetRolesAsync(user);

			List<AssignRoleViewModel> model = new List<AssignRoleViewModel>();
			foreach (var item in roles)
			{
				AssignRoleViewModel role = new AssignRoleViewModel
				{
					Id = item.Id,
					Name = item.Name,
					RoleExist = userRoles.Contains(item.Name)
				};
				model.Add(role);
			}
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AssignRole(List<AssignRoleViewModel> model)
		{
			if (TempData["UserId"] == null) return RedirectToAction("Index");

			var userId = TempData["UserId"].ToString();
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return NotFound();

			foreach (var item in model)
			{
				if (item.RoleExist)
				{
					await _userManager.AddToRoleAsync(user, item.Name);
				}
				else
				{
					await _userManager.RemoveFromRoleAsync(user, item.Name);
				}
			}
			return RedirectToAction("Index");
		}
	}
}
