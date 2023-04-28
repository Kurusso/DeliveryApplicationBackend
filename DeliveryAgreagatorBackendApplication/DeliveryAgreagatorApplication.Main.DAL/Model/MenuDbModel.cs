using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class MenuDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public Guid RestaurantId { get; set; }  
        public RestaurantDbModel Restaurant { get; set; }
        public ICollection<DishDbModel> Dishes { get; set; }

        public MenuDbModel(MenuDTO model, Guid restaurantId) 
        {
            Id = new Guid();
            Name = model.Name;
            isActive = model.IsActive;
            RestaurantId = restaurantId;
        }
        public MenuDbModel() { }
        public MenuShortDTO ConvertToDTO()
        {
            var model = new MenuShortDTO
            {
                Id = Id,
                Name = Name,
                IsActive = isActive
            };
            return model;
        }
    }
}
