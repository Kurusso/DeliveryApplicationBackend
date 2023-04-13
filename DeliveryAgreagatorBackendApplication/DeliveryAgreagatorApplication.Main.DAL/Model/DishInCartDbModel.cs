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

        public DishInCartDbModel(DishInCartDbModel model, Guid orderId) 
        {
            Id = Guid.NewGuid();
            DishId= model.DishId;
            CustomerId= model.CustomerId;
            OrderId= orderId;
            Active = true;
            Counter = model.Counter;
        }
        public DishInCartDbModel() { }

        public DishInCartDTO ConvertToDTO()
        {
            var model = new DishInCartDTO { 
                Counter = this.Counter, 
                Id = this.Id,
                Dish = this.Dish.ConvertToDTO()
            };
            return model;
        }
    }
}
