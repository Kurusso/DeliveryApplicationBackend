using DeliveryAgreagatorBackendApplication.Models;

namespace DeliveryAgreagatorBackendApplication.Model
{
    public class ManagerDbModel
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public RestaurantDbModel Restaurant { get; set; }
    }
}
