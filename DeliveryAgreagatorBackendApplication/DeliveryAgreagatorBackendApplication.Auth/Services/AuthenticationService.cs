using DeliveryAgreagatorBackendApplication.Auth.Models;
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


        public async Task<TokenPair> Login(LoginDTO model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == model.Login && BCrypt.Net.BCrypt.HashPassword(model.Password) == x.PasswordHash);         
            if (user == null)
            {
                throw new ArgumentException ("Invalid username or password!");
            }
            try
            {
                var tokenPair = await _tokenSerivce.GenerateTokenPair(user);
                _context.RefreshTokens.Add(new RefreshTokenDb { Id = tokenPair.RefreshTokenId, Token = tokenPair.RefreshToken, Expires =tokenPair.RefreshExpires})
                return tokenPair;
            }
            catch
            {
                throw;
            }          
        }
    }
}
