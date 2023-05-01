﻿using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<RestaurantShortDTO> GetRestaurantById(Guid restaurantId)
        {
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x=>x.Id== restaurantId);
            if(restaurant == null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Restaurant, restaurantId);
            }
            return restaurant.ConvertToDTO();
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
