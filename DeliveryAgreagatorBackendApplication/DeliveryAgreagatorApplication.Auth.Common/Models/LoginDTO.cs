using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorApplication.Auth.Common.Models
{
    public class LoginDTO
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
