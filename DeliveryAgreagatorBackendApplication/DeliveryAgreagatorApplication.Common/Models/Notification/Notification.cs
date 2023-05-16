using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Models.Notification.Enums;


namespace DeliveryAgreagatorApplication.Common.Models.Notification
{
    public class Notification
    {
        public Guid UserId { get; set; }
        public int OrderNumber { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string Text { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public Notification(Guid userId, int orderNumber, OrderStatus orderStatus, NotificationStatus notificationStatus=0) 
        {
            UserId= userId;
            OrderNumber= orderNumber;
            NotificationStatus= notificationStatus;
            Text = $"Your order №{orderNumber} ";
            switch (orderStatus)
            {
                case OrderStatus.Kitchen:
                    Text += "is being prepared!";
                    break;
                case OrderStatus.Packaging:
                    Text += "is being packed!";
                    break;
                case OrderStatus.Packed:
                    Text+= "is packed!";
                    break;
                case OrderStatus.Delivery:
                    Text+= "is being delivered";
                    break;
                case OrderStatus.Delivered:
                    Text += "is delivered";
                    break;
                case OrderStatus.Canceled:
                    Text+= "was canceled by courier!";
                    break;
            }
        }
    }
}
