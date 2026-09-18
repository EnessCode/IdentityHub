using IdentityHub.Context;
using IdentityHub.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityHub.Controllers
{
	[Authorize(Roles = "Admin")]
	public class CategoryController : Controller
	{
		private readonly IdentityContext _context;

		public CategoryController(IdentityContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _context.Categories.ToListAsync();
			return View(categories);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Category category)
		{
			if (!ModelState.IsValid) return View(category);

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Delete(int id)
		{
			var category = await _context.Categories.FindAsync(id);
			if (category != null)
			{
				_context.Categories.Remove(category);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			var category = await _context.Categories.FindAsync(id);

			if (category == null) return NotFound();

			return View(category);
		}

		[HttpPost]
		public async Task<IActionResult> Update(Category category)
		{
			if (!ModelState.IsValid) return View(category);

			_context.Categories.Update(category);
			await _context.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}