namespace DeliveryAgreagatorBackendApplication.Auth.Models
{
    public class Courier
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
