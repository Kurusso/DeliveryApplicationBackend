namespace DeliveryAgreagatorApplication.API.Common.Models.DTO
{
    public class DishInCartDTO
    {
        public Guid Id { get; set; }
        public DishDTO Dish { get; set; }
        public int Counter { get; set; }
        public int? PriceWhenOpdered { get; set; }
    }
}
