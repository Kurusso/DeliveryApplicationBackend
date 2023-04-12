using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public interface IMenuService
    {
        public Task<List<MenuShortDTO>> GetRestaurantMenus(Guid restaurantId, bool active);
        public Task<List<DishDTO>> GetMenuDishes(Guid restaurantId, Guid menuId, List<Category> categories, bool? isVegetarian, DishFilter? filter, int page);
    }
}
