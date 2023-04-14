using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorApplication.Main.BL.Services
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
                throw new WrongIdException(WrongIdExceptionSubject.Dish, dishId, $"in this {restaurantId} restaurant");
            }
            else
            {
                return dish.ConvertToDTO();
            }
        }

        public async Task SetRating(Guid restaurantId, Guid dishId, Guid userId, int rating) 
        {
            var dish = await _context.Dishes.Include(x=>x.Ratings).FirstOrDefaultAsync(x => x.Id == dishId && x.RestaurantId == restaurantId);
            if (dish == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Dish, dishId, $"in this {restaurantId} restaurant");
            }
            var orderedDish = await _context.DishInCart.Include(c=>c.Order).FirstOrDefaultAsync(x => x.CustomerId == userId && x.DishId == dishId && x.Active == false && x.Order.Status == Status.Delivered);
            if(orderedDish == null)
            {
                throw new InvalidOperationException($"You have never ordered this {dishId} dish!"); 
            }
            var ratingFromDb = await _context.Ratings.FirstOrDefaultAsync(x => x.DishId == dishId && x.CustomerId == userId);
            if (ratingFromDb != null)
            {
                ratingFromDb.Value = rating;
            }
            else
            {
               var newRating = new RatingDbModel { CustomerId = userId, DishId = dishId, Value = rating, Id = new Guid() };
               await _context.Ratings.AddAsync(newRating);
               dish.Rating = dish.Ratings.Sum(x => x.Value) / dish.Ratings.Count();

            }

            await _context.SaveChangesAsync();

        }
    }
}
