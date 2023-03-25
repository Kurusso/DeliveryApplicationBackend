using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public class MenuService : IMenuService
    {
        private readonly BackendDbContext _context;
        public MenuService(BackendDbContext context)
        {
            _context = context;
        }

        public Task<List<DishDTO>> GetMenuDishes(Guid restaurantId, Guid menuId, Category[] categories, bool isVegetarian, DishFilter filter, int page)
        {
            
        }

        public async Task<List<MenuShortDTO>> GetRestaurantMenus(Guid restaurantId, bool active)
        {
            var restaurant = await _context.Restaurants
	        .Include(r => r.Menus)
	        .FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant == null)
            {
                throw new ArgumentException($"There is no restaurant with {restaurantId} id!");
            }
            else {
                var menus = active ? restaurant.Menus.Where(c=>c.isActive).ToList() : restaurant.Menus.ToList();
                var menusDTO = menus.Select(x =>new MenuShortDTO(x)).ToList();
                return menusDTO;
			}
		}
    }
}
