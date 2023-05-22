using DeliveryAgreagatorBackendApplication.Model;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorApplication.Main.DAL
{
    public class BackendDbContext: DbContext 
    {
        public DbSet<CookDbModel> Cooks { get; set; }
        public DbSet<DishDbModel> Dishes { get; set; }
        public DbSet<DishInCartDbModel> DishInCart { get; set; }
        public DbSet<MenuDbModel> Menus { get; set; }
        public DbSet<OrderDbModel> Orders { get; set; }
        public DbSet<RatingDbModel> Ratings { get; set; }
        public DbSet<RestaurantDbModel> Restaurants { get; set; }
        public DbSet<ManagerDbModel> Managers { get; set; }
        public BackendDbContext(DbContextOptions<BackendDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDbModel>()
                .HasOne(o => o.Cook)
                .WithMany()
                .HasForeignKey(o => o.CookId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrderDbModel>()
                .HasOne(o => o.Restaurant)
                .WithMany()
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DishDbModel>()
                .HasOne(o => o.Restaurant)
                .WithMany()
                .HasForeignKey(o => o.RestaurantId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
