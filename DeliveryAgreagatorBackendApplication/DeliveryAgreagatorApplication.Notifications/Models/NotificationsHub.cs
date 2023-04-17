using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.Models
{
    [Authorize]
    public class NotificationsHub : Hub { }
}
