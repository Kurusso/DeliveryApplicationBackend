using DeliveryAgreagatorApplication.Common.Models.Notification.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Common.Models.Notification
{
    public class Notification
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }
    }
}
