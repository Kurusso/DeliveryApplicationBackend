using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
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

        public async Task AddDishToMenu(Guid managerId, Guid menuId, Guid dishId)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(x => x.Id == managerId);
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == manager.RestaurantId);
            var menu = restaurant.Menus.FirstOrDefault(x => x.Id == menuId);
            if (menu == null)
            {
                throw new ArgumentException($"You haven't got access to menu with this {menuId} id!"); //TODO: разбить на несколько эксепшенов
            }
            var dish = await _context.Dishes.FirstOrDefaultAsync(x=> x.Id == dishId);
            if (dish==null)
            {
                throw new WrongIdException(WrongIdExceptionSubject.Dish, dishId);
            }
            if (dish.RestaurantId != restaurant.Id)
            {
                throw new InvalidOperationException("You can't add dish from another restaurant in this restaurant menu!");
            }
            menu.Dishes.Add(dish);
            await _context.SaveChangesAsync();
        }

        public async Task CreateDish(Guid managerId, Guid menuId, DishPostDTO dishPost)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(x=>x.Id==managerId);
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == manager.RestaurantId);
            var menu = restaurant.Menus.FirstOrDefault(x => x.Id == menuId);
            if (menu == null)
            {
                throw new ArgumentException($"You haven't got access to menu with this {menuId} id!"); //TODO: разбить на несколько эксепшенов
            }
            menu.Dishes.Add(new DishDbModel(dishPost));
            await _context.SaveChangesAsync();
        }

        public async Task EditDish(Guid managerId, Guid dishId, DishPutDTO dishPut)
        {
            var manager = await _context.Managers.FirstOrDefaultAsync(x => x.Id == managerId);
            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(x => x.Id == manager.RestaurantId);
            var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId);
            if (dish.RestaurantId != restaurant.Id)
            {
                throw new InvalidOperationException("You can't edit dish from another restaurant!");
            }
            dish.Name= dishPut.Name==null ? dish.Name : dishPut.Name;
            dish.Price = dishPut.Price == null ? dish.Price : (int)dishPut.Price;
            dish.IsVegetarian = dishPut.IsVegetarian == null ? dish.IsVegetarian : (bool)dishPut.IsVegetarian;
            dish.PhotoUrl = dishPut.PhotoUrl == null ? dish.PhotoUrl : dishPut.PhotoUrl;
            dish.Description = dishPut.Description == null ? dish.Description : dishPut.Description;
            dish.Category = dishPut.Category == null ? dish.Category : (Category)dishPut.Category;
            dish.IsActive = dishPut.isActive == null ? dish.IsActive : (bool)dishPut.isActive;
            await _context.SaveChangesAsync();

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
