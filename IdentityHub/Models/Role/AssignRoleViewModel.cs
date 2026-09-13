using System.ComponentModel.DataAnnotations;

namespace IdentityHub.Models.Role
{
	public class AssignRoleViewModel
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public bool RoleExist { get; set; }
	}
}
