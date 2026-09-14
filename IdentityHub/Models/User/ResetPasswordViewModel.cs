using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.User
{
	public class ResetPasswordViewModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string Token { get; set; }

		[Required(ErrorMessage = "Lütfen yeni şifrenizi giriniz.")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }

		[Required(ErrorMessage = "Lütfen şifrenizi doğrulayınız.")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Şifreler birbiriyle uyuşmuyor.")]
		public string ConfirmPassword { get; set; }
	}
}