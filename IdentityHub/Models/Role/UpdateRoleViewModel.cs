using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.Role
{
	public class UpdateRoleViewModel
	{
		public string Id { get; set; }

		[Required(ErrorMessage = "Lütfen yeni rol adını giriniz.")]
		[MaxLength(20, ErrorMessage = "Rol adı en fazla 20 karakter olabilir.")]
		public string Name { get; set; }
	}
}