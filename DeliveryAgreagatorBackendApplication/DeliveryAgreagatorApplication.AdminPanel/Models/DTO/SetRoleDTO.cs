using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.DTO
{
	public class SetRoleDTO
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public Role Role { get; set; }

		[Required]
		public RoleAction Action { get; set; }

		public Guid? RestaurantId { get; set; }
	}
}
