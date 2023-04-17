using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("IdClaim").Value;
        }
    }
}
