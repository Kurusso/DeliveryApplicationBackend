namespace DeliveryAgreagatorBackendApplication.Models
{
    public class CookDbModel
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public RestaurantDbModel Restaurant { get; set; }

    }
}
