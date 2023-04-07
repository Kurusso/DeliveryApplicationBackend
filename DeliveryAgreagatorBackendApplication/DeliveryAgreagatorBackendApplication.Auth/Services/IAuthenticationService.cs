using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public interface IAuthenticationService
    {
        public Task<TokenPair> Login (LoginDTO model);
    }
}
