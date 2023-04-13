
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Auth.DAL.Models;

namespace DeliveryAgreagatorApplication.Auth.Common.Interfaces
{
    public interface ITokenSerivce
    {
        public Task<TokenPair> GenerateTokenPair(ApplicationUser user); //TODO: не импорить DAL в Common

    }
}
