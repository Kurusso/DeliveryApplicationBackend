using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public interface IAuthenticationService
    {
        public Task<TokenPairDTO> Login (LoginDTO model);
        public Task<TokenPairDTO> Refresh(ClaimsPrincipal user);
        public Task<TokenPairDTO> Register(RegisterDTO model);
    }
}
