using DeliveryAgreagatorBackendApplication.Models;
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

        public async Task AddDishToCart( Guid dishId, Guid userId)
        {
            var dish = await _context.Dishes.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == dishId);
            if (dish == null)
            {
                throw new ArgumentException($"There is no dish with this {dishId} id !");
            }
            var dishInCart = await _context.DishInCart.FirstOrDefaultAsync(x => x.DishId == dishId && x.CustomerId == userId && x.Active);
            if (dishInCart != null)
            {
                dishInCart.Counter++;
            }
            else
            {
               var newDishInCart = new DishInCartDbModel
                {
                    Id = new Guid(),
                    Active = true,
                    DishId = dishId,
                    CustomerId = userId,
                    Counter = 1,
                };
                await _context.DishInCart.AddAsync(newDishInCart);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrDecreaseDishInCart( Guid dishId, Guid userId, bool deacrease = false)
        {
            var dish = await _context.Dishes.Include(x => x.Ratings).FirstOrDefaultAsync(x => x.Id == dishId);
            if (dish == null)
            {
                throw new ArgumentException($"There is no dish with this {dishId} id!");
            }
            var dishInCart = await _context.DishInCart.FirstOrDefaultAsync(x => x.DishId == dishId && x.CustomerId == userId && x.Active);
            if (dishInCart == null) 
            {
                throw new ArgumentException($"There is no dish with this {dishId} id in your cart!");
            }
            if (!deacrease)
            {
                 _context.Remove(dishInCart);
            }
            else
            {
                dishInCart.Counter--;
                if(dishInCart.Counter == 0) { _context.Remove(dishInCart); }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<DishInCartDTO>> GetCart(Guid userId)
        {
            var dishes = _context.DishInCart.Include(x=>x.Dish).ThenInclude(c=>c.Ratings).Where(x=>x.CustomerId==userId && x.Active==true).ToList();
            var dishesDTO = dishes.Select(x => new DishInCartDTO(x)).ToList();
            return dishesDTO;
        }
    }
}
