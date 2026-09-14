using IdentityHub.Entities;
using IdentityHub.Models.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityHub.Service.TokenService
{
	public class TokenService : ITokenService
	{
		private readonly JwtSettings _jwtSettings;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IOptions<JwtSettings> jwtSettings, UserManager<AppUser> userManager)
		{
			_jwtSettings = jwtSettings.Value;
			_userManager = userManager;
		}

		public async Task<UserTokenViewModel> CreateTokenAsync(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Name, user.UserName),               
				new Claim(ClaimTypes.GivenName, user.Name),             
				new Claim(ClaimTypes.Surname, user.Surname),             
				new Claim(ClaimTypes.Locality, user.City),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var roles = await _userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _jwtSettings.Issuer,
				audience: _jwtSettings.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
				signingCredentials: creds
			);

			return new UserTokenViewModel
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				Name = user.Name,
				Surname = user.Surname,
				City = user.City,
				Username = user.UserName
			};
		}
	}
}