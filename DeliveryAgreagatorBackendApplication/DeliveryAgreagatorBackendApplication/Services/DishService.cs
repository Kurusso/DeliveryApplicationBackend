﻿using DeliveryAgreagatorBackendApplication.Models;
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

        public async Task SetRating(Guid restaurantId, Guid dishId, Guid userId, int rating)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId && x.RestaurantId == restaurantId);
            if (dish == null)
            {
                throw new ArgumentException($"There is no dish with this {dishId} id in this {restaurantId} restaurant!");
            }
            var orderedDish = await _context.DishInCart.FirstOrDefaultAsync(x => x.CustomerId == userId && x.DishId == dishId && x.Active == false);
            if(orderedDish == null)
            {
                throw new ArgumentNullException($"You have never ordered this {dishId} dish!"); //TODO:Добавить кастомное исключение.
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
            }

            await _context.SaveChangesAsync();

        }
    }
}