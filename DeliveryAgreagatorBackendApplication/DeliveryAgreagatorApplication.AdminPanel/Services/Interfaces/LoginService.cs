using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;
        public LoginService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _context= context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<LoginStatusDTO> VerifyLogin(LoginDTO loginModel)
        {
            var status = new LoginStatusDTO { IsLogedIn = true };
            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if(user == null)
            {
                status.IsLogedIn= false;
                status.DenyReason = "Incorrect Email!";
            }
            if (user != null)
            {
                var passwordVerified = BCrypt.Net.BCrypt.Verify(loginModel.Password, user.PasswordHash);
                if (!passwordVerified)
                {
                    status.IsLogedIn = false;
                    status.DenyReason = "Incorrect Password!";
                }
                else
                {
                    var admin = await _context.Admins.FirstOrDefaultAsync(x => x.UserId == user.Id);
                    if (admin == null)
                    {
                        status.IsLogedIn = false;
                        status.DenyReason = "It is not Admin account!";
                    }
                }
            }          
            if (status.IsLogedIn)
            {
                status.User = user;
                return status;
            }
            else
            {
                return status;
            }
        }
    }
}
