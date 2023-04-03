using DeliveryAgreagatorBackendApplication.Model;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorBackendApplication
{
    public class BackendDbContext: DbContext 
    {
        public DbSet<CookDbModel> Cooks { get; set; }
        public DbSet<CourierDbModel> Couriers { get; set; }
        public DbSet<CustomerDbModel> Customers { get; set; }
        public DbSet<DishDbModel> Dishes { get; set; }
        public DbSet<DishInCartDbModel> DishInCart { get; set; }
        public DbSet<MenuDbModel> Menus { get; set; }
        public DbSet<OrderDbModel> Orders { get; set; }
        public DbSet<RatingDbModel> Ratings { get; set; }
        public DbSet<RestaurantDbModel> Restaurants { get; set; }
        public DbSet<ManagerDbModel> Managers { get; set; }
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }


    }
}
