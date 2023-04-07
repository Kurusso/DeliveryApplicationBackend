namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public class TokenPair
    {
        public Guid RefreshTokenId { get; set; }
        public DateTime RefreshExpires { get; set; }
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
