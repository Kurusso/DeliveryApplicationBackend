namespace DeliveryAgreagatorApplication.Auth.DAL.Models
{
    public class Customer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? Address { get; set; }
    }
}
