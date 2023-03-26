using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IDishService
    {
        public Task<DishDTO> GetDish(Guid restaurantId, Guid dishId);
    }
}
