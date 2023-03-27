using DeliveryAgreagatorBackendApplication.Models;
using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface ICartService
    {
        public Task<List<DishInCartDTO>> GetCart(Guid userId);
        public Task AddDishToCart(Guid dishId, Guid userId);
        public Task DeleteOrDecreaseDishInCart( Guid dishId, Guid userId, bool deacrease = false);
    }
}
