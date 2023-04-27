using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class DishDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsActive { get; set; }
        public int Rating { get; set; }
        public string PhotoUrl { get; set; }
        public Guid RestaurantId { get; set; }
        public RestaurantDbModel Restaurant { get; set; }
        public Category Category { get; set; }
        public ICollection<MenuDbModel> Menus { get; set; }
        public ICollection<RatingDbModel> Ratings { get; set; }

        public DishDbModel() { }
        public DishDbModel(DishPostDTO model, Guid restaurantId)
        {
            Id = new Guid();
            Name = model.Name;
            Description = model.Description;
            Price = model.Price;
            IsVegetarian = model.IsVegetarian;
            PhotoUrl = model.PhotoUrl;
            RestaurantId = restaurantId;
        }
        public DishDTO ConvertToDTO()
        {
            var model = new DishDTO
            {
                Id = this.Id,
                Category = this.Category,
                Description = this.Description,
                Price = this.Price,
                IsVegetarian = this.IsVegetarian,
                Name = this.Name,
                PhotoUrl = this.PhotoUrl,
                Rating = this.Rating
            };
            return model;
        }
    }
}
