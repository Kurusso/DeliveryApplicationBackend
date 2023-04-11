using DeliveryAgreagatorBackendApplication.Auth.Models;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public interface ITokenSerivce
    {
        public Task<TokenPair> GenerateTokenPair(ApplicationUser user);

    }
}
