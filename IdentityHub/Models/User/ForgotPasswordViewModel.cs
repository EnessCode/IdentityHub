using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.User
{
	public class ForgotPasswordViewModel
	{
		[Required(ErrorMessage = "Lütfen kayıtlı e-posta adresinizi giriniz.")]
		[EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi formatı giriniz.")]
		public string Email { get; set; }
	}
}
