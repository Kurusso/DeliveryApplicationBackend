using DeliveryAgreagatorApplication.Auth.Common.Models.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorApplication.Auth.Common.Models
{
    public class RegisterDTO
    {
        [Required]
        [EmailAddress]
        public string Login { get; set; }

        [Required]
        [MinLength(10)]
        public string Password { get; set; }

        [Required]
        [MinLength(10)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public Gender Gender { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
