using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class RatingDbModel
    {
        public Guid Id { get; set; }

        [Range(1, 10)]
        public int Value { get; set; } 

        public Guid DishId { get; set; }
        public DishDbModel Dish { get; set; }

        public Guid CustomerId { get; set; }
    }
}
