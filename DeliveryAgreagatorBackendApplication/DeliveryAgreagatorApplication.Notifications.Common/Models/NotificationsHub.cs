using DeliveryAgreagatorApplication.Common.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.Common.Models
{
    [Authorize]
    public class NotificationsHub : Hub 
    {
		public override Task OnConnectedAsync()
		{
			Context.Items["userId"] = Context.User.Identity.Name;
			return base.OnConnectedAsync();
		}

	}
}
