using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.DTO
{
    public class RestaurantPostDTO
    {
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Picture { get; set; }
    }
}
