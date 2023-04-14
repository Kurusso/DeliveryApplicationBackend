using DeliveryAgreagatorApplication.API.Common.Models.Enums;

namespace DeliveryAgreagatorApplication.API.Common.Models.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
        public ICollection<DishInCartDTO>? DishesInCart { get; set; }
    }
}
