using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IDishService
    {
        public Task<DishDTO> GetDish(Guid restaurantId, Guid dishId);
        public Task SetRating(Guid restaurantId, Guid dishId, Guid userId, int rating);
        public Task CreateDish(Guid managerId, Guid menuId, DishPostDTO dishPost);
        public Task EditDish(Guid managerId, Guid dishId, DishPutDTO dishPut);
        public Task AddDishToMenu(Guid managerId, Guid menuId, Guid dishId);
    }
}
