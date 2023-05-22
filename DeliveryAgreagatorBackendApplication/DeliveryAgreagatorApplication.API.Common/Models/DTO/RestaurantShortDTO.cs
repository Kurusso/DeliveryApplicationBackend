using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorApplication.API.Common.Models.DTO
{
    public class RestaurantShortDTO
    {
        public Guid Id { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [StringLength(300, MinimumLength = 3)]
        [Required]
        public string Picture { get; set; }
    }
}
