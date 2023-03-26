using DeliveryAgreagatorBackendApplication.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorBackendApplication.Services
{
    public class CartService : ICartService
    {
        private readonly BackendDbContext _context;

        public CartService(BackendDbContext context)
        {
            _context = context;
        }

        public async Task<List<DishInCartDTO>> GetCart(Guid userId)
        {
            var dishes = _context.DishInCart.Include(x=>x.Dish).Where(x=>x.CustomerId==userId && x.Active==true).ToList();
            var dishesDTO = dishes.Select(x => new DishInCartDTO(x)).ToList();
            return dishesDTO;
        }
    }
}
