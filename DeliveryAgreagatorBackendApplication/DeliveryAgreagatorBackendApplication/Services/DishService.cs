using DeliveryAgreagatorBackendApplication.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public class DishService : IDishService
    {
        private readonly BackendDbContext _context;

        public DishService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task<DishDTO> GetDish(Guid restaurantId, Guid dishId)
        {
            var dish = await _context.Dishes.Include(x=>x.Ratings).FirstOrDefaultAsync(x=>x.Id==dishId && x.RestaurantId==restaurantId);
            if (dish == null)
            {
                throw new ArgumentException($"There is no dish with this {dishId} id in this {restaurantId} restaurant!");
            }
            else
            {
                return new DishDTO(dish);
            }
        }
    }
}
