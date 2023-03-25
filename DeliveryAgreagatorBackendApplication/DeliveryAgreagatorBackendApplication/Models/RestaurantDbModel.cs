using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class RestaurantDbModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Picture { get; set; }
        public ICollection<CookDbModel> Cooks { get; set; }
        public ICollection<MenuDbModel> Menus { get; set; }
    }
}
