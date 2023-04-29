using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public interface IRestaurantService
    {
        public Task<List<RestaurantShortDTO>> GetRestaurants();
        public Task PostRestaurant(RestaurantPostDTO restaurant);
        public Task DeleteRestaurant(Guid restaurantId);
        public Task<RestaurantShortDTO> GetRestaurantById(Guid restaurantId);
    }
}
