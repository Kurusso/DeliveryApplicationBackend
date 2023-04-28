using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Auth.DAL;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace DeliveryAgreagatorApplication.Auth.BL.Services
{
    public class TokenSerivce : ITokenSerivce
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        public TokenSerivce(AuthDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager= roleManager;
            _context = context;
            _userManager = userManager;
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user, bool isrefresh, Guid refreshTokenId) //TODO: добавить все нужные клэймы
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim> {
            new Claim("IdClaim", user.Id.ToString()),
            new Claim("RefreshIdClaim", refreshTokenId.ToString()),
            new Claim("TokenTypeClaim", isrefresh ? "Refresh" : "Access")
            };
            if (roles.Any(x => x == "Customer"))
            {
               var customer = await _context.Customers.FirstOrDefaultAsync(x => x.UserId == user.Id);
               claims.Add(new Claim("Address", customer.Address));
            }
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);

                    foreach (var claim in roleClaims)
                    {
                        claims.Add(claim);
                    }
                }
            
            }
            return claims;
        }
        public async Task<TokenPair> GenerateTokenPair(IdentityUser<Guid> user)
        {
            Guid refreshTokenId = Guid.NewGuid();  
            var accessClaims = await GetClaims((ApplicationUser)user, false, refreshTokenId);
            var refreshClaims = await GetClaims((ApplicationUser)user, true, refreshTokenId);
            int refreshlifetime = JwtConfigurations.RefreshLifetime;
            int accesslifetime = JwtConfigurations.Lifetime;
            var now = DateTime.UtcNow;
            var access = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: accessClaims,
                expires: now.AddMinutes(accesslifetime),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var refresh = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: refreshClaims,
                expires: now.AddMinutes(refreshlifetime),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(access);
            var encodedRefreshJwt = new JwtSecurityTokenHandler().WriteToken(refresh);
            return new TokenPair { AccessToken = encodedAccessJwt, RefreshToken= encodedRefreshJwt, RefreshTokenId = refreshTokenId, RefreshExpires = now.AddMinutes(refreshlifetime) };
        }

        public async Task<string> GenerateAccessToken(IdentityUser<Guid> user, Guid refreshTokenId)
        {
            var accessClaims = await GetClaims((ApplicationUser)user, false, refreshTokenId);
            int accesslifetime = JwtConfigurations.Lifetime;
            var now = DateTime.UtcNow;
            var access = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: accessClaims,
                expires: now.AddMinutes(accesslifetime),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedAccessJwt = new JwtSecurityTokenHandler().WriteToken(access);
            return encodedAccessJwt;
        }
    }
}
