
using DeliveryAgreagatorApplication.API.Common.Models.Enums;

namespace DeliveryAgreagatorApplication.API.Common.Models.DTO
{
    public class DishDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public float Rating { get; set; }
        public bool IsVegetarian { get; set; }
        public string PhotoUrl { get; set; }
        public Category Category { get; set; }

    }
}
