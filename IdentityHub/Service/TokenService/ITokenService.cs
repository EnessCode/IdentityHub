using IdentityHub.Entities;
using IdentityHub.Models.Jwt;

namespace IdentityHub.Service.TokenService
{
	public interface ITokenService
	{
		Task<UserTokenViewModel> CreateTokenAsync(AppUser user);
	}
}
