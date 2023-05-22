using DeliveryAgreagatorApplication.Common.Models.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IRabbitMqService
    {
       public Task SendMessage(Notification message);
    }
}
