using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DeliveryAgreagatorBackendApplication.Auth
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<Cook> Cooks { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<RefreshTokenDb> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Cook)
                .WithOne(c => c.User)
                .HasForeignKey<Cook>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.User)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Manager)
                .WithOne(c => c.User)
                .HasForeignKey<Manager>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Courier)
                .WithOne(c => c.User)
                .HasForeignKey<Courier>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasNoKey();
        }
 
    }
}
