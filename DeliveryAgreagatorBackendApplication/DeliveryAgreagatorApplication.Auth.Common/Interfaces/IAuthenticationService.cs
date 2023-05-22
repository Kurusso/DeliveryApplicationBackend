using DeliveryAgreagatorApplication.Auth.Common.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace DeliveryAgreagatorApplication.Auth.Common.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<TokenPairDTO> Login (LoginDTO model);
        public Task<string> Refresh(ClaimsPrincipal user);
        public Task<TokenPairDTO> Register(RegisterDTO model);
    }
}
