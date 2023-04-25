using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.Common.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("IdClaim").Value;
        }
    }
}
