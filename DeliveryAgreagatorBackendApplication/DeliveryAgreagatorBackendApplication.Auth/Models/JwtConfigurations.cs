using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public class JwtConfigurations
    {
        public const string Issuer = "JwtTestIssuer";  
        public const string Audience = "JwtTestClient"; 
        private const string Key = "SuperSecretKeyBazingaLolKek!*228322";   
        public const int Lifetime = 10;
        public const int RefreshLifetime = 300;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }

    }
}
