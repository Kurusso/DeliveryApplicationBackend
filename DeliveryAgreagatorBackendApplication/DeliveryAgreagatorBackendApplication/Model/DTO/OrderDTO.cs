using DeliveryAgreagatorBackendApplication.Models.Enums;

namespace DeliveryAgreagatorBackendApplication.Models.DTO
{
    public class OrderDTO
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public int Price { get; set; }
        public string Address { get; set; }
        public Status Status { get; set; }
        public ICollection<DishInCartDTO> DishesInCart { get; set; }
        public OrderDTO(OrderDbModel model) 
        {
            Id= model.Id;
            DeliveryTime= model.DeliveryTime;
            OrderTime= model.OrderTime;
            Price= model.Price;
            Address= model.Address;
            Status = model.Status;
            DishesInCart = model.DishesInCart.Select(x => new DishInCartDTO(x)).ToList();
        }
    }
}
