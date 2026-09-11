using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.User
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
		public string Password { get; set; }
	}
}
