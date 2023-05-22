using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.DTO
{
    public class RestaurantViewModel
    {
        public RestaurantPostDTO? Post { get; set; }
        public List<RestaurantShortDTO>? Get { get; set; }
    }
}
