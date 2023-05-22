using DeliveryAgreagatorApplication.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Main.Common.Models.DTO
{
    public class StatusDTO
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
