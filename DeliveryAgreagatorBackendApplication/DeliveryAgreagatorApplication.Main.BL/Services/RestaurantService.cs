using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DeliveryAgreagatorApplication.Main.BL.Services
{
    public class RestaurantService: IRestaurantService
    {
        private readonly int _pageSize = 5;
		private int _pageCount = 5;
		private string _regexp = "";
		private BackendDbContext _context;
        public RestaurantService(BackendDbContext dbContext) 
        { 
        _context= dbContext;
		}
        public async Task<List<RestaurantShortDTO>> GetRestaurants(int page, string? name, int? pageSize=null)
        {
			int pageCount = pageSize==null? _pageCount : (int)pageSize;
			if(pageSize == null)
			{
				pageSize = _pageSize;
			}
			if (name != null) {
				_regexp = name;
			}
			if ((_context.Restaurants.Count() % pageSize) == 0)
			{
                pageCount = (_context.Restaurants.Count() / (int)pageSize);
			}
			else
			{
                pageCount = (_context.Restaurants.Count() / (int)pageSize) + 1;
			}
			if (page > pageCount || page <= 0)
			{
				throw new ArgumentOutOfRangeException($"{page} is incorrect page number!");
			}
			var Restaurants = _context.Restaurants.Where(
			c => Regex.IsMatch(c.Name, _regexp))
			.Skip((int)pageSize * (page-1)).Take((int)pageSize).ToList();

			var RestaurantsDTO = Restaurants.Select(x => x.ConvertToDTO()).ToList();
			return RestaurantsDTO;
		}

    }
}
