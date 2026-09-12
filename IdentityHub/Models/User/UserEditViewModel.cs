using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.User
{
	public class UserEditViewModel
	{
		public string Username { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public string Email { get; set; }
		public string City { get; set; }
		public string ImageUrl { get; set; }
		public string PhoneNumber { get; set; }
		public string? Password { get; set; }

		[Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
		public string? ConfirmPassword { get; set; }
	}
}
