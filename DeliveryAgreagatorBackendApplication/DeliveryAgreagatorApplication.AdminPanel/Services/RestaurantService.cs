using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;

namespace DeliveryAgreagatorApplication.AdminPanel.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly BackendDbContext _context;
        public RestaurantService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task DeleteRestaurant(Guid restaurantId)
        {
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
             _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task<List<RestaurantShortDTO>> GetRestaurants()
        {
            var restaurants = _context.Restaurants.ToList();
            return restaurants.Select(x=>x.ConvertToDTO()).ToList();
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
    }
}
