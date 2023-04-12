using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface ICartService
    {
        public Task<List<DishInCartDTO>> GetCart(Guid userId);
        public Task AddDishToCart(Guid dishId, Guid userId);
        public Task DeleteOrDecreaseDishInCart( Guid dishId, Guid userId, bool deacrease = false);
    }
}
