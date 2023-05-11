using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public interface IRestaurantAdminService
    {
        public Task<int> GetRestaurantsCount(string name);
        public Task PostRestaurant(RestaurantPostDTO restaurant);
        public Task DeleteRestaurant(Guid restaurantId);
        public Task<RestaurantShortDTO> GetRestaurantById(Guid restaurantId);
        public Task PutRestaurant(RestaurantShortDTO restaurant);

    }
}
