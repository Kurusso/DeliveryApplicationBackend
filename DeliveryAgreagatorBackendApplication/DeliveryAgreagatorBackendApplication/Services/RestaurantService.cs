using DeliveryAgreagatorBackendApplication.Models.DTO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public class RestaurantService:IRestaurantService
    {
        private readonly int _pageSize = 6;
		private int _pageCount = 6;
		private string _regexp = "";
		private BackendDbContext _context;
        public RestaurantService(BackendDbContext dbContext) 
        { 
        _context= dbContext;
		}
        public async Task<List<RestaurantShortDTO>> GetRestaurants(int page, string? name)
        {
			if (name != null) {
				_regexp = name;
			}
			if ((_context.Restaurants.Count() % _pageSize) == 0)
			{
				_pageCount = (_context.Restaurants.Count() / _pageSize);
			}
			else
			{
				_pageCount = (_context.Restaurants.Count() / _pageSize) + 1;
			}
			if (page > _pageCount || page <= 0)
			{
				throw new ArgumentOutOfRangeException($"{page} is incorrect page number!");
			}
			var Restaurants = _context.Restaurants.Where(
			c => Regex.IsMatch(c.Name, _regexp))
			.Skip(_pageSize*(page-1)).Take(_pageSize).ToList();

			var RestaurantsDTO = Restaurants.Select(x => new RestaurantShortDTO(x)).ToList();
			return RestaurantsDTO;
		}

    }
}
