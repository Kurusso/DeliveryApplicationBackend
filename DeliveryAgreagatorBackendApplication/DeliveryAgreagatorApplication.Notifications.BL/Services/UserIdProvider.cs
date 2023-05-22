using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace DeliveryAgreagatorApplication.Notifications.Common.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            string? jwt = connection.GetHttpContext().Request.Headers.Authorization.ToString()?.Replace("Bearer ", "");
            if (jwt != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(jwt);
                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "IdClaim")?.Value;
                return userId;
            }
            return null;
        }
    }
}
