namespace DeliveryAgreagatorBackendApplication.Models
{
    public class MenuDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool isOld { get; set; }
        public RestaurantDbModel Restaurant { get; set; }
        public ICollection<DishDbModel> Dishes { get; set; }

    }
}
