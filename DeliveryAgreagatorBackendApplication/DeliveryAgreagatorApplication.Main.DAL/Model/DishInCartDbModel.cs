using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class DishInCartDbModel
    {
        public Guid Id { get; set; }

        public Guid DishId { get; set; }
        public DishDbModel Dish { get; set; }

        public Guid CustomerId { get; set; }

        public Guid? OrderId { get; set; }
        public OrderDbModel? Order { get; set; }
        public bool Active { get; set; }
        public int Counter { get; set; }
        public int? PriceWhenOpdered { get; set; }
        public DishInCartDbModel(DishInCartDbModel model, Guid orderId) 
        {
            Id = Guid.NewGuid();
            DishId= model.DishId;
            CustomerId= model.CustomerId;
            OrderId= orderId;
            Active = model.Active;
            Counter = model.Counter;
            PriceWhenOpdered = model.Dish.Price;
        }
        public DishInCartDbModel() { }

        public DishInCartDTO ConvertToDTO()
        {
            var model = new DishInCartDTO { 
                Counter = this.Counter, 
                Id = this.Id,
                PriceWhenOpdered= this.PriceWhenOpdered,
                Dish = this.Dish.ConvertToDTO()
            };
            return model;
        }
    }
}
