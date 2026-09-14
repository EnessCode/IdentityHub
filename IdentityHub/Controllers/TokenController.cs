using IdentityHub.Entities;
using IdentityHub.Service.TokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityHub.Controllers
{
	[Authorize]
	public class TokenController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public TokenController(UserManager<AppUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null) return NotFound();

			var tokenModel = await _tokenService.CreateTokenAsync(user);

			return View(tokenModel);
		}
	}
}