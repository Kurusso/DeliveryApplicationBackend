using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Auth.BL.Services
{
    public class ProfileService : IProfileService
    {
        private readonly AuthDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        public ProfileService(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task ChangeProfile(Guid userId, ProfileDTO model)
        {
            Customer? customer = null;
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var occupied = await _context.Users.FirstOrDefaultAsync(x => x.Id != userId && (x.UserName == model.UserName || x.PhoneNumber == model.Phone || x.Email == model.Email));
            if (occupied != null)
            {
                throw new InvalidOperationException("Something is already taken!"); //TODO: create normal exception
            }
            if (user.CustomerId != null)
            {
                customer = await _context.Customers.FindAsync(user.CustomerId);
                customer.Address = model.Address==null ? customer.Address : model.Address;
            }
            user.BirthDate = model.BirthDate == null ? user.BirthDate : (DateTime)model.BirthDate; 
            user.Email = model.Email == null ? user.Email : model.Email;
            user.UserName = model.UserName == null ? user.UserName : model.UserName;
            user.PhoneNumber = model.Phone == null ? user.PhoneNumber : model.Phone;
            await _context.SaveChangesAsync();
        }

        public async Task<ProfileDTO> GetProfile(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            Customer? customer=null;
            if (user.CustomerId != null)
            {
                 customer = await _context.Customers.FindAsync(user.CustomerId);
            }
            var userDTO = new ProfileDTO 
            { 
                BirthDate= user.BirthDate, 
                Email= user.Email, 
                Phone=user.PhoneNumber, 
                UserName = user.UserName, 
                Address = customer==null ? null : customer.Address,
            };
            return userDTO;
        }

        public async Task UpdatePassword(Guid userId, PasswordUpdateDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var correctOldPass = BCrypt.Net.BCrypt.Verify(model.OldPassword, user.PasswordHash);
            if(!correctOldPass)
            {
                throw new Exception("Wrong password!");
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();
        }
    }
}
