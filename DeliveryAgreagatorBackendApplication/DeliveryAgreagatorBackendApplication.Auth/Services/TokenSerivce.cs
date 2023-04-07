using DeliveryAgreagatorBackendApplication.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace DeliveryAgreagatorBackendApplication.Auth.Services
{
    public class TokenSerivce : ITokenSerivce
    {
        private string GenerateAccessToken(ApplicationUser user, ulong refreshTokenId)
        {
            return "";
        }
        private List<Claim> GetClaims(ApplicationUser user, bool isrefresh, Guid refreshTokenId) //TODO: добавить все нужные клэймы
        {
            var claims = new List<Claim>
            {
                new Claim("IdClaim", user.Id.ToString()),
                new Claim("RefreshIdClaim", refreshTokenId.ToString()),
                new Claim("TokenTypeClaim", isrefresh? "Refresh" : "Access")
            };
            return claims;
        }
        public async Task<TokenPair> GenerateTokenPair(ApplicationUser user)
        {
            Guid refreshTokenId = Guid.NewGuid();  
            var accessClaims = GetClaims(user, false, refreshTokenId);
            var refreshClaims = GetClaims(user, true, refreshTokenId);
            int refreshlifetime = JwtConfigurations.RefreshLifetime;
            int accesslifetime = JwtConfigurations.Lifetime;
            var now = DateTime.UtcNow;
            var access = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: now,
                claims: accessClaims,
                expires: now.AddMinutes(5000),
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
    }
}
