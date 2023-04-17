
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using Microsoft.AspNetCore.Identity;

namespace DeliveryAgreagatorApplication.Auth.Common.Interfaces
{
    public interface ITokenSerivce
    {
        public Task<TokenPair> GenerateTokenPair(IdentityUser<Guid> user); //TODO: не импорить DAL в Common

    }
}
