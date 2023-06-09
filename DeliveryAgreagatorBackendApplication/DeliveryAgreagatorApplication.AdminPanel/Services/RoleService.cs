﻿using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.DAL;
using DeliveryAgreagatorBackendApplication.Model;
using DeliveryAgreagatorBackendApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;

namespace DeliveryAgreagatorApplication.AdminPanel.Services
{
    public class RoleService : IRoleService
    {
        private readonly AuthDbContext _authDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly BackendDbContext _backendDbContext;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public RoleService(AuthDbContext authDbContext, BackendDbContext backendDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _backendDbContext = backendDbContext;
            _authDbContext = authDbContext;
            _roleManager = roleManager;
        }

        private async Task GiveUserARole(SetRoleDTO model, IList<string> roles, ApplicationUser userAuth, RestaurantDbModel restaurant)
        {
            if (roles.Any(x => x == model.Role.ToString()))
            {
                throw new InvalidOperationException("This user already have this role!");
            }
            await _userManager.AddToRoleAsync(userAuth, model.Role.ToString());
            switch (model.Role)
            {
                case Role.Manager:

                    if (restaurant == null && model.RestaurantId != null)
                    {
                        throw new WrongIdException(WrongIdExceptionSubject.Restaurant, (Guid)model.RestaurantId);
                    }
                    else if (restaurant == null)
                    {
                        throw new ArgumentNullException("RestaurantId is required!");
                    }
                    else
                    {
                        await _backendDbContext.Managers.AddAsync(new ManagerDbModel { Id = userAuth.Id, RestaurantId = restaurant.Id });
                        var managerId = Guid.NewGuid();
                        await _authDbContext.Managers.AddAsync(new Manager { Id = managerId, UserId = userAuth.Id });
                        userAuth.ManagerId = managerId;
                    }
                    break;

                case Role.Cook:
                    if (restaurant == null && model.RestaurantId != null)
                    {
                        throw new WrongIdException(WrongIdExceptionSubject.Restaurant, (Guid)model.RestaurantId);
                    }
                    else if (restaurant == null)
                    {
                        throw new ArgumentNullException("RestaurantId is required!");
                    }
                    else
                    {
                        await _backendDbContext.Cooks.AddAsync(new CookDbModel { Id = userAuth.Id, RestaurantId = restaurant.Id });
                        var cookId = Guid.NewGuid();
                        await _authDbContext.Cooks.AddAsync(new Cook { Id = cookId, UserId = userAuth.Id });
                        userAuth.CookId = cookId;
                    }
                    break;

                case Role.Courier:
                    var courierId = Guid.NewGuid();
                    await _authDbContext.Couriers.AddAsync(new Courier { Id = courierId, UserId = userAuth.Id });
                    userAuth.CourierId = courierId;
                    break;
            }

        }
        private async Task RemoveUserRole(SetRoleDTO model, IList<string> roles, ApplicationUser userAuth, RestaurantDbModel restaurant)
        {
            if (roles.All(x => x != model.Role.ToString()))
            {
                throw new InvalidOperationException($"This user haven't got tole with this {model.Role} name!");
            }
            await _userManager.RemoveFromRoleAsync(userAuth, model.Role.ToString());
            switch (model.Role)
            {
                case Role.Manager:
                    var managerAuth = await _authDbContext.Managers.FirstOrDefaultAsync(x => x.UserId == userAuth.Id);
                    var managerBackend = await _backendDbContext.Managers.FirstOrDefaultAsync(x => x.Id == userAuth.Id);
                    _authDbContext.Managers.Remove(managerAuth);
                    userAuth.ManagerId = null;
                    _backendDbContext.Managers.Remove(managerBackend);
                    break;
                case Role.Cook:
                    var cookAuth = await _authDbContext.Cooks.FirstOrDefaultAsync(x => x.UserId == userAuth.Id);
                    var cookBackend = await _backendDbContext.Cooks.FirstOrDefaultAsync(x => x.Id == userAuth.Id);
                    _authDbContext.Cooks.Remove(cookAuth);
                    userAuth.CookId = null;
                    _backendDbContext.Cooks.Remove(cookBackend);
                    break;
                case Role.Courier:
                    var courier = await _authDbContext.Couriers.FirstOrDefaultAsync(x => x.UserId == userAuth.Id);
                    _authDbContext.Couriers.Remove(courier);
                    userAuth.CourierId = null;
                    break;
            }
        }
        public async Task EditUserRoles(SetRoleDTO model)
        {
            using (var transaction = _authDbContext.Database.BeginTransaction())
            {
                try
                {
                    var userAuth = await _userManager.FindByEmailAsync(model.Email);
                    var restaurant = model.RestaurantId == null ? null : await _backendDbContext.Restaurants.FindAsync(model.RestaurantId);
                    if (userAuth == null)
                    {
                        throw new ArgumentException($"There is no user with this {model.Email} email!");
                    }
                    var roles = await _userManager.GetRolesAsync(userAuth);
                    if (model.Action == RoleAction.Give)
                    {
                        try
                        {
                            await GiveUserARole(model, roles, userAuth, restaurant);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    if (model.Action == RoleAction.Retrive)
                    {
                        try
                        {
                            await RemoveUserRole(model, roles, userAuth, restaurant);
                        }
                        catch
                        {
                            throw;
                        }
                    }
                    if (model.Action == RoleAction.Ban)
                    {
                        userAuth.Baned = true;
                    }
                    if (model.Action == RoleAction.Unban)
                    {
                        userAuth.Baned = false;
                    }
                    var refreshes = await _authDbContext.RefreshTokens.Where(x => x.Expires < DateTime.UtcNow).ToListAsync();
                    _authDbContext.RefreshTokens.RemoveRange(refreshes);
                    await _authDbContext.SaveChangesAsync();
                    await _backendDbContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
             }
        }

        public async Task<List<UserDTO>> GetUsersWithRole(Guid? restaurantId, Role role)
        {
            if (restaurantId != null)
            {
                
                switch (role) {
                    case Role.Courier:
                        throw new InvalidOperationException("There is no need of restaurantId to get couriers!");
                    case Role.Cook:
                        var cooksId = await _backendDbContext.Cooks.Where(x=>x.RestaurantId == restaurantId).Select(x => x.Id).ToListAsync();
                        var cooks =  await _authDbContext.Users.Where(x=>cooksId.Contains(x.Id)).ToListAsync(); 
                        return cooks.Select(x => new UserDTO { Id = x.Id, Email = x.Email, Name = x.UserName }).ToList();                        
                    case Role.Manager:
                        var managersId = await _backendDbContext.Managers.Where(x => x.RestaurantId == restaurantId).Select(x=>x.Id).ToListAsync();
                        var managers = await _authDbContext.Users.Where(x => managersId.Contains(x.Id)).ToListAsync();
                        return managers.Select(x => new UserDTO { Id = x.Id, Email = x.Email, Name = x.UserName }).ToList();                      
                }                             
            }
            var users = await _userManager.GetUsersInRoleAsync(role.ToString());
            return users.Select(x => new UserDTO { Id = x.Id, Email = x.Email, Name = x.UserName }).ToList();
        }
    }
}
