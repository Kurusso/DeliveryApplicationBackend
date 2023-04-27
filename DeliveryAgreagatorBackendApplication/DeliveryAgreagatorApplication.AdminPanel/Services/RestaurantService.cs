using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.DAL;

namespace DeliveryAgreagatorApplication.AdminPanel.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly BackendDbContext _context;
        public RestaurantService(BackendDbContext context)
        {
            _context = context;
        }
        public async Task<List<RestaurantShortDTO>> GetRestaurants()
        {
            var restaurants = _context.Restaurants.ToList();
            return restaurants.Select(x=>x.ConvertToDTO()).ToList();
        }
    }
}
