namespace DeliveryAgreagatorBackendApplication.Models
{
    public class CustomerDbModel
    {
        public Guid Id { get; set; }
        public ICollection<RatingDbModel> Ratings { get; set; }
    }
}
