using Microsoft.IdentityModel.Tokens;
using System.Text;


    public class JwtConfigurations
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }   
        public int Lifetime { get; set; }
        public int RefreshLifetime { get; set; }
    }

