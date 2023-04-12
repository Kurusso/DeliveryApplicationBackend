using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IRestaurantService
    {
        public Task<List<RestaurantShortDTO>> GetRestaurants(int page,string? name);
    }
}
