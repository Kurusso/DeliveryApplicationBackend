using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthDbContext _context;
        private ITokenSerivce _tokenSerivce;
        public AuthenticationService(AuthDbContext context, ITokenSerivce tokenSerivce) 
        {
        _context= context;
        _tokenSerivce= tokenSerivce;
        }


        public async Task<TokenPairDTO> Login(LoginDTO model)
        {
            var pss = BCrypt.Net.BCrypt.HashPassword(model.Password);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Login); 
            bool correctPassword = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);
            if (user == null || !correctPassword)
            {
                throw new ArgumentException ("Invalid username or password!");
            }
            try
            {
                var tokenPair = await _tokenSerivce.GenerateTokenPair(user);
                await _context.RefreshTokens.AddAsync(new RefreshTokenDb { Id = tokenPair.RefreshTokenId, Token = tokenPair.RefreshToken, Expires = tokenPair.RefreshExpires, UserId =user.Id });
                await _context.SaveChangesAsync();
                return new TokenPairDTO { AccessToken=tokenPair.AccessToken, RefreshToken = tokenPair.RefreshToken};
            }
            catch
            {
                throw;
            }          
        }

        public async Task<TokenPairDTO> Refresh(ClaimsPrincipal user)
        {
            var refreshTokenId = user.Claims.FirstOrDefault(c => c.Type == "RefreshIdClaim").Value;
            var userId = user.Claims.FirstOrDefault(c => c.Type == "IdClaim").Value;
            Guid refreshTokenGuidId;
            Guid userGuidId;
            Guid.TryParse(refreshTokenId, out refreshTokenGuidId);
            Guid.TryParse(userId, out userGuidId);
            if (refreshTokenId == null || userId==null)
            {
                throw new ArgumentException("Incorrect token!");
            }
            var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshTokenGuidId && x.Expires>DateTime.UtcNow);
            var userDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == userGuidId);
            if (refreshToken == null || userDb==null)
            {
                throw new InvalidOperationException("Token is not actual or correct!");
            }
            _context.RefreshTokens.Remove(refreshToken);
            try
            {
               var tokenPair = await _tokenSerivce.GenerateTokenPair(userDb);
               await _context.RefreshTokens.AddAsync(new RefreshTokenDb { Id = tokenPair.RefreshTokenId, Token = tokenPair.RefreshToken, Expires = tokenPair.RefreshExpires, UserId=userGuidId });
               await _context.SaveChangesAsync();
               return new TokenPairDTO { AccessToken= tokenPair.AccessToken, RefreshToken= tokenPair.RefreshToken };
            }
            catch
            {
                throw;
            }
        }

        public async Task<TokenPairDTO> Register(RegisterDTO model)
        {
            var userId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var user = new ApplicationUser { Id = userId, Email=model.Login, EmailConfirmed=true, PasswordHash=BCrypt.Net.BCrypt.HashPassword(model.Password), PhoneNumber=model.PhoneNumber, UserName = model.FullName, BirthDate = model.BirthDate, PhoneNumberConfirmed=true, TwoFactorEnabled=false, AccessFailedCount=0, CustomerId = customerId };
            var customer = new Customer { Id = customerId, Address = model.Address, UserId = userId };
            await _context.Customers.AddAsync(customer);
            await _context.Users.AddAsync(user);
            try
            {
                var tokenPair = await _tokenSerivce.GenerateTokenPair(user);
                await _context.RefreshTokens.AddAsync(new RefreshTokenDb { Id = tokenPair.RefreshTokenId, Token = tokenPair.RefreshToken, Expires = tokenPair.RefreshExpires, UserId = userId });
                await _context.SaveChangesAsync();
                return new TokenPairDTO { AccessToken = tokenPair.AccessToken, RefreshToken = tokenPair.RefreshToken };
            }
            catch
            {
                throw;
            }
        }
    }
}
