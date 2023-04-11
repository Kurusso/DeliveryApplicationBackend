using DeliveryAgreagatorBackendApplication.Models.Enums;

namespace DeliveryAgreagatorBackendApplication.Models.DTO
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

		public DishDTO(DishDbModel dish) {
			Id= dish.Id;
			Name= dish.Name;
			Description= dish.Description;
			Price= dish.Price;
			IsVegetarian= dish.IsVegetarian;
			PhotoUrl= dish.PhotoUrl;
			Category = dish.Category;
			Rating = dish.Rating;
        }
	}
}
