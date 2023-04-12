using DeliveryAgreagatorApplication.API.Common.Models.DTO;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class MenuDbModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool isActive { get; set; }
        public RestaurantDbModel Restaurant { get; set; }
        public ICollection<DishDbModel> Dishes { get; set; }

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
