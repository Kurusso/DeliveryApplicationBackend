using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;

namespace DeliveryAgreagatorApplication.Main.Common.Interfaces
{
    public interface IMenuService
    {
        public Task<List<MenuShortDTO>> GetRestaurantMenus(Guid restaurantId, bool active);
        public Task<List<DishDTO>> GetMenuDishes(Guid restaurantId, Guid menuId, List<Category> categories, bool? isVegetarian, DishFilter? filter, int page);
        public Task AddMenu (MenuDTO menu, Guid managerId);
        public Task EditMenu (Guid menuId,MenuDTO menu, Guid managerId);
    }
}
