using IdentityHub.Entities;
using IdentityHub.Models.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityHub.Controllers
{
	[Authorize(Roles = "Admin")]
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _roleManager;

		public RoleController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var roles = await _roleManager.Roles.ToListAsync();
			return View(roles);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(CreateRoleViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			IdentityRole role = new IdentityRole
			{
				Name = model.Name
			};
			await _roleManager.CreateAsync(role);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null) return NotFound();

			await _roleManager.DeleteAsync(role);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			if (string.IsNullOrEmpty(id)) return NotFound();

			var role = await _roleManager.FindByIdAsync(id);
			if (role == null) return NotFound();

			UpdateRoleViewModel model = new UpdateRoleViewModel
			{
				Id = role.Id,
				Name = role.Name
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Update(UpdateRoleViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var role = await _roleManager.FindByIdAsync(model.Id);
			if (role == null) return NotFound();

			role.Name = model.Name;

			await _roleManager.UpdateAsync(role);
			return RedirectToAction("Index");
		}
	}
}
