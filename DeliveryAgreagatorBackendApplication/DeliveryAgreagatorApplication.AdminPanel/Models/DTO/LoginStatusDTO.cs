using DeliveryAgreagatorApplication.Auth.DAL.Models;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.DTO
{
    public class LoginStatusDTO
    {
        public bool IsLogedIn { get; set; }
        public ApplicationUser? User { get; set; }
        public string? DenyReason { get; set; } 
    }
}
