using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public class RolesAndClaimsInit
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<AuthDbContext>();

                string[] roles = new string[] { "Customer", "Manager", "Cook", "Courier" };
                List<Claim> customerClaims = new List<Claim> 
                { 
                    new Claim("ViewCart", "Allow"), 
                    new Claim("CartOperations", "Allow"), 
                    new Claim("SetRating", "Allow"), 
                    new Claim("ViewCart", "Allow") ,
                    new Claim("GetOrders", "Customer"),
                    new Claim("OrderOperation", "Customer")
                };
                List<Claim> cookClaims = new List<Claim>
                {
                    new Claim("GetOrders", "Cook"),
                    new Claim("OrderOperation", "Cook")
                };
                List<Claim> courierClaims = new List<Claim>
                {
                    new Claim("GetOrders", "Courier"),
                    new Claim("OrderOperation", "Courier")
                };
                List<Claim> managerClaims = new List<Claim>
                {
                    new Claim("GetOrders", "Manager"),
                };
                foreach (string role in roles)
                {
                    RoleManager<IdentityRole<Guid>> _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();

                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        var roleModel = new IdentityRole<Guid>(role);
                        await _roleManager.CreateAsync(roleModel);
                        switch (role)
                        {
                            case "Customer":
                                foreach (var claim in customerClaims) 
                                {
                                    await _roleManager.AddClaimAsync(roleModel, claim);
                                };
                                break;
                            case "Cook":
                                foreach (var claim in cookClaims)
                                {
                                    await _roleManager.AddClaimAsync(roleModel, claim);
                                };
                                break;
                            case "Courier":
                                foreach (var claim in courierClaims)
                                {
                                    await _roleManager.AddClaimAsync(roleModel, claim);
                                };
                                break;
                            case "Manager":
                                foreach (var claim in managerClaims)
                                {
                                    await _roleManager.AddClaimAsync(roleModel, claim);
                                };
                                break;
                        }
                    }
                }
                await context.SaveChangesAsync();
            }
        }
    }
}
