using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Auth.Common.Models
{
    public class PasswordUpdateDTO
    {
        [Required]
        [MinLength(10)]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(10)]
        public string NewPassword { get; set; }
    }
}
