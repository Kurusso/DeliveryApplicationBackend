using DeliveryAgreagatorBackendApplication.Models.Enums;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class DishDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsActive { get; set; }
        public string PhotoUrl { get; set; }
        public Guid RestaurantId { get; set; }
        public RestaurantDbModel Restaurant { get; set; }
        public Category Category { get; set; }
        public ICollection<MenuDbModel> Menus { get; set; }
        public ICollection<RatingDbModel> Ratings { get; set; }
    }
}
