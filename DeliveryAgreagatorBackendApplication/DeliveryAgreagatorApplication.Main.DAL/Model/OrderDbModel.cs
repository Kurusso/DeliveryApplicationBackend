using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Models.Enums;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class OrderDbModel
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public int Price { get; set; } 
        public string Address { get; set; }
        public OrderStatus Status { get; set; }

        public Guid? RestaurantId { get; set; }
        public RestaurantDbModel? Restaurant { get; set; }
        public Guid CustomerId { get; set; }

        public Guid? CourierId { get; set; }

        public Guid? CookId { get; set; }
        public CookDbModel? Cook { get; set; }

        public ICollection<DishInCartDbModel> DishesInCart { get; set;}
        public OrderDbModel(OrderDbModel model)
        {
            var guid = Guid.NewGuid();
            Id = guid;
            RestaurantId = model.RestaurantId;
            Number= Math.Abs(guid.GetHashCode());
            DeliveryTime = model.DeliveryTime;
            OrderTime = DateTime.UtcNow;
            Address = model.Address;
            Status = OrderStatus.Created; 
            CustomerId = model.CustomerId;
            DishesInCart = model.DishesInCart.Select(x => new DishInCartDbModel(x, guid)).ToList();
            Price = model.DishesInCart.Sum(x => x.Dish.Price * x.Counter);
        }
        public OrderDbModel() { }

        public OrderDTO ConvertToDTO()
        {
            var model = new OrderDTO
            {
                Id = Id,
                Status = Status,
                DeliveryTime = DeliveryTime,
                Address = Address,
                Number = Number,
                OrderTime = OrderTime,
                Price = Price,
                DishesInCart = (this.DishesInCart != null && this.DishesInCart.Any()) ? this.DishesInCart.Select(x => x.ConvertToDTO()).ToList() : null
            };
            return model;
        }
    }
}
