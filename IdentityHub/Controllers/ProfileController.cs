using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.Controllers
{
	public class ProfileController : Controller
	{
		public IActionResult EditProfile()
		{
			return View();
		}
	}
}
