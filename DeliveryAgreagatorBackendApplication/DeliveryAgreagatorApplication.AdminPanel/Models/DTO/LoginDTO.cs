using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.DTO
{
    public class LoginDTO
    {

            [Required]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

    }
}
