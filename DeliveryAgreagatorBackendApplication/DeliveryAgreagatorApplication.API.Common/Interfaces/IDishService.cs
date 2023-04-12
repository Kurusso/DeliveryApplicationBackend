using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IDishService
    {
        public Task<DishDTO> GetDish(Guid restaurantId, Guid dishId);
        public Task SetRating(Guid restaurantId, Guid dishId, Guid userId, int rating);
    }
}
