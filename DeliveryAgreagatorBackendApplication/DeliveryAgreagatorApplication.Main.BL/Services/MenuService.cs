using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeliveryAgreagatorApplication.Main.BL.Services
{
    public class MenuService : IMenuService
    {
        private readonly int _pageSize = 5;
        private int _pageCount = 5;
        private readonly BackendDbContext _context;
        public MenuService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task<List<DishDTO>> GetMenuDishes(Guid restaurantId, Guid menuId, List<Category> categories, bool? isVegetarian, DishFilter? filter, int page)
        {
            var restaurant = await _context.Restaurants.Include(c=>c.Menus).ThenInclude(x=>x.Dishes).ThenInclude(x=>x.Ratings).FirstOrDefaultAsync(x=>x.Id==restaurantId);
            if (restaurant == null) {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurantId);
            }
            var menu = restaurant.Menus.FirstOrDefault(x => x.Id == menuId);
            if (menu == null) {
                throw new WrongIdException(WrongIdExceptionSubject.Menu, menuId);
            }
            var dishes = menu.Dishes.Where(dish => isVegetarian == true ? dish.IsVegetarian : true).Where(dish => categories.Count == 0 ? true : categories.Any(c => c == dish.Category)).Where(x=>x.IsActive);

            var dishesDTO = dishes.Select(x => x.ConvertToDTO());

            if ((dishesDTO.Count() % _pageSize) == 0)
            {
                _pageCount = (dishesDTO.Count() / _pageSize);
            }
            else
            {
                _pageCount = (dishesDTO.Count() / _pageSize) + 1;
            }
            if (page > _pageCount || page <= 0)
            {
                throw new ArgumentOutOfRangeException($"{page} is incorrect page number!");
            }

            if (filter == DishFilter.PriceAsc)
            {
                dishesDTO = dishesDTO.OrderBy(c => c.Price);
            }

            if (filter == DishFilter.PriceDesc)
            {
                dishesDTO = dishesDTO.OrderByDescending(c => c.Price);
            }

            if (filter == DishFilter.NameAsc)
            {
                dishesDTO = dishesDTO.OrderBy(c => c.Name);
            }

            if (filter == DishFilter.NameDesc)
            {
                dishesDTO = dishesDTO.OrderByDescending(c => c.Name);
            }

            if (filter == DishFilter.RatingAsc)
            {
                dishesDTO = dishesDTO.OrderBy(c => c.Rating);
            }

            if (filter == DishFilter.RatingDesc)
            {
                dishesDTO = dishesDTO.OrderByDescending(c => c.Rating);
            }

             var dishesDTOPaginated = dishesDTO
            .Skip(_pageSize * (page - 1)).Take(_pageSize).ToList();
            return dishesDTOPaginated;
        }

        public async Task<List<MenuShortDTO>> GetRestaurantMenus(Guid restaurantId, bool active)
        {
            var restaurant = await _context.Restaurants
	        .Include(r => r.Menus)
	        .FirstOrDefaultAsync(r => r.Id == restaurantId);
            if (restaurant == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurantId);
            }
            else {
                var menus = active ? restaurant.Menus.Where(c=>c.isActive).ToList() : restaurant.Menus.ToList();
                var menusDTO = menus.Select(x =>x.ConvertToDTO()).ToList();
                return menusDTO;
			}
		}

        public async Task AddMenu(MenuDTO menu, Guid managerId)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(x=>x.Id== managerId);
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x=>x.Id== manager.RestaurantId);

            await _context.Menus.AddAsync(new MenuDbModel(menu, restaurant.Id));
            await _context.SaveChangesAsync();
        }

        public async Task EditMenu(Guid menuId, MenuDTO menu, Guid managerId)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(x => x.Id == managerId);
            var restaurant = await _context.Restaurants.Include(x=>x.Menus).FirstOrDefaultAsync(x => x.Id == manager.RestaurantId);
            var menuToChange = restaurant.Menus.FirstOrDefault(x => x.Id == menuId);
            if (menuToChange == null)
            {
                throw new ArgumentException($"You haven't got access to menu with this {menuId} id!"); //TODO: разбить на несколько эксепшенов
            }
            menuToChange.isActive = menu.IsActive;
            menuToChange.Name = menu.Name;
            await _context.SaveChangesAsync();
        }
    }
}
