namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public class RefreshTokenDb
    {
        public Guid Id { get; set; }
        public string Token { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Expires { get; set; }
    }
}
