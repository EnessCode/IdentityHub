using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.User
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Lütfen adınızı giriniz.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
		public string Surname { get; set; }

		[Required(ErrorMessage = "Lütfen kullanıcı adınızı giriniz.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Lütfen mail adresinizi giriniz.")]
		[EmailAddress(ErrorMessage = "Lütfen geçerli bir mail adresi giriniz.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Lütfen şifre tekrarını giriniz.")]
		[Compare("Password", ErrorMessage = "Girdiğiniz şifreler birbiriyle uyuşmuyor.")]
		public string ConfirmPassword { get; set; }
	}
}
