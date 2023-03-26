using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IMenuService
    {
        public Task<List<MenuShortDTO>> GetRestaurantMenus(Guid restaurantId, bool active);
        public Task<List<DishDTO>> GetMenuDishes(Guid restaurantId, Guid menuId, List<Category> categories, bool? isVegetarian, DishFilter? filter, int page);
    }
}
