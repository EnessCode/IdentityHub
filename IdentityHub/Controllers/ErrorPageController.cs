using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.Controllers
{
	public class ErrorPageController : Controller
	{
		public IActionResult Page404()
		{
			return View();
		}

		public IActionResult Page401()
		{
			return View();
		}

		public IActionResult Page403()
		{
			return View();
		}
	}
}
