namespace DeliveryAgreagatorBackendApplication.Models.DTO
{
    public class DishInCartDTO
    {
        public Guid Id { get; set; }
        public DishDbModel Dish { get; set; }
        public int Counter { get; set; }

        public DishInCartDTO(DishInCartDbModel model) {
            Id = model.Id;
            Dish = model.Dish;
            Counter = model.Counter;
        }
    }
}
