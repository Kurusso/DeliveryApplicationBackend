using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace DeliveryAgreagatorApplication.AdminPanel.Services
{
    public class RestaurantAdminService : IRestaurantAdminService
    {
        private string _regexp = "";
        private readonly BackendDbContext _context;
        private readonly AuthDbContext _authContext;
        private readonly UserManager<ApplicationUser> _userManager;
        public RestaurantAdminService(BackendDbContext context, AuthDbContext authContext, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _authContext= authContext;
            _context = context;
        }

        public async Task DeleteRestaurant(Guid restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if(restaurant == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurantId);
            }
            var cooksId = await _context.Cooks.Where(x => x.RestaurantId == restaurantId).Select(x => x.Id).ToListAsync();
            var cooks = await _authContext.Users.Where(x => cooksId.Contains(x.Id)).ToListAsync();

            var managersId = await _context.Managers.Where(x => x.RestaurantId == restaurantId).Select(x => x.Id).ToListAsync();
            var managers = await _authContext.Users.Where(x => managersId.Contains(x.Id)).ToListAsync();

            foreach(var manager in managers)
            {
                await _userManager.RemoveFromRoleAsync(manager, Role.Manager.ToString());
                var managerToDelete = await _authContext.Managers.FirstOrDefaultAsync(x => x.UserId == manager.Id);
                manager.ManagerId = null;
                _authContext.Managers.Remove(managerToDelete);
            }
            foreach(var cook in cooks)
            {
                await _userManager.RemoveFromRoleAsync(cook, Role.Cook.ToString());
                var cookToDelete = await _authContext.Cooks.FirstOrDefaultAsync(x => x.UserId == cook.Id);
                cook.CookId = null;
                _authContext.Cooks.Remove(cookToDelete);
            }
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            await _authContext.SaveChangesAsync();
        }

        public async Task<RestaurantShortDTO> GetRestaurantById(Guid restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x=>x.Id== restaurantId);
            if(restaurant == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurantId);
            }
            return restaurant.ConvertToDTO();
        }

        public async Task<int> GetRestaurantsCount(string? name)
        {
            if (name != null)
            {
                _regexp = name;
            }
            var restaurants = _context.Restaurants.Where(x=> Regex.IsMatch(x.Name, _regexp)).Count();
            return restaurants;
        }


        public async Task PostRestaurant(RestaurantPostDTO restaurant)
        {
            var restaurantDb = new RestaurantDbModel
            {
                Id = new Guid(),
                Name = restaurant.Name,
                Picture = restaurant.Picture
            };
            await _context.Restaurants.AddAsync(restaurantDb);
            await _context.SaveChangesAsync();
        }

        public async Task PutRestaurant(RestaurantShortDTO restaurant)
        {
            var restaurantDb = await _context.Restaurants.FirstOrDefaultAsync(x=>x.Id==restaurant.Id);
            if (restaurantDb == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurant.Id);
            }
            restaurantDb.Name = restaurant.Name;
            restaurantDb.Picture = restaurant.Picture;
            await _context.SaveChangesAsync();
        }
    }
}
