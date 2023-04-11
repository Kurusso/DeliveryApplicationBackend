using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public class LoginDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
