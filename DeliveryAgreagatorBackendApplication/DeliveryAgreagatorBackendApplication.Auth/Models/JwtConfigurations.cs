using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public interface JwtConfigurations
    {
        public const string Issuer = "JwtTestIssuer";  
        public const string Audience = "JwtTestClient"; 
        private const string Key = "gewkjfwekofewokokewkgowepflw";   
        public const int Lifetime = 60;
        public const int RefreshLifetime = 300;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }

    }
}
