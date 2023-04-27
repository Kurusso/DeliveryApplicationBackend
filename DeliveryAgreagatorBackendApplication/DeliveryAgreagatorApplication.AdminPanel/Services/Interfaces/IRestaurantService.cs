using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public interface IRestaurantService
    {
        public Task<List<RestaurantShortDTO>> GetRestaurants();

    }
}
