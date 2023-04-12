using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IRestaurantService
    {
        public Task<List<RestaurantShortDTO>> GetRestaurants(int page,string? name);
    }
}
