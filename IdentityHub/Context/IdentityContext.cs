using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityHub.Entities;

namespace IdentityHub.Context
{
	public class IdentityContext : IdentityDbContext<AppUser>
	{
		public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
		{
		}
	}
}
