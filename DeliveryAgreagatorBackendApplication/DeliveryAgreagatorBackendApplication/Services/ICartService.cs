using DeliveryAgreagatorBackendApplication.Models;
using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface ICartService
    {
        public Task<List<DishInCartDTO>> GetCart(Guid userId);
    }
}
