namespace DeliveryAgreagatorBackendApplication.Models.DTO
{
    public class DishInCartDTO
    {
        public Guid Id { get; set; }
        public DishDTO Dish { get; set; }
        public int Counter { get; set; }

        public DishInCartDTO(DishInCartDbModel model) {
            Id = model.Id;
            Dish = new DishDTO(model.Dish);
            Counter = model.Counter;
        }
    }
}
