using DeliveryAgreagatorBackendApplication.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IDishService
    {
        public Task<DishDTO> GetDish(Guid restaurantId, Guid dishId);
        public Task SetRating(Guid restaurantId, Guid dishId, Guid userId, int rating);
    }
}
