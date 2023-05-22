using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Common.Models.Notification.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Notifications.DAL.Models
{
	public class NotificationDbModel
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public int OrderNumber { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public string Text { get; set; }
		public NotificationStatus NotificationStatus { get; set; }

		public NotificationDbModel(Notification notification)
		{
			Id = new Guid();
			UserId = notification.UserId;
			OrderNumber = notification.OrderNumber;
			OrderStatus = notification.OrderStatus;
			Text = notification.Text;
			NotificationStatus = notification.NotificationStatus;
		}
	}

}
