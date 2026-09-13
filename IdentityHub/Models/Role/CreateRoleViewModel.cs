using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.Role
{
	public class CreateRoleViewModel
	{
		[Required(ErrorMessage = "Lütfen oluşturmak istediğiniz rolün adını giriniz.")]
		[MaxLength(20, ErrorMessage = "Rol adı en fazla 20 karakter olabilir.")]
		public string Name { get; set; }
	}
}
