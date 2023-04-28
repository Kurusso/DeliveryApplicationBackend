
using DeliveryAgreagatorApplication.Auth.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace DeliveryAgreagatorApplication.Auth.Common.Interfaces
{
    public interface ITokenSerivce
    {
        public Task<TokenPair> GenerateTokenPair(IdentityUser<Guid> user); 
        public Task<string> GenerateAccessToken(IdentityUser<Guid> user, Guid refreshTokenId);
    }
}
