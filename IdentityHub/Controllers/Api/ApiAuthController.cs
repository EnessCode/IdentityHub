using IdentityHub.Entities;
using IdentityHub.Models.User;
using IdentityHub.Service.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityHub.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApiAuthController : ControllerBase
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;

		public ApiAuthController(UserManager<AppUser> userManager, ITokenService tokenService)
		{
			_userManager = userManager;
			_tokenService = tokenService;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (!ModelState.IsValid)
				return BadRequest("Geçersiz veri gönderildi.");

			var user = await _userManager.FindByNameAsync(model.Username);
			if (user == null)
				return Unauthorized("Kullanıcı bulunamadı.");

			var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
			if (!checkPassword)
				return Unauthorized("Şifre hatalı.");

			if (!user.IsActive)
				return BadRequest("Hesabınız pasife alınmış.");

			var token = await _tokenService.CreateTokenAsync(user);

			return Ok(new
			{
				Message = "Giriş başarılı",
				Token = token
			});
		}

		[HttpGet("secured-data")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
		public IActionResult GetSecuredData()
		{
			return Ok(new
			{
				Message = "Tebrikler! Geçerli bir JWT ile bu gizli veriye eriştiniz.",
				Data = "Bu veriyi sadece Admin olan mobil veya web API istemcileri görebilir."
			});
		}
	}
}