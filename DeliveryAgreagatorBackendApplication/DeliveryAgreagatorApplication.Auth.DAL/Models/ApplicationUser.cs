using Microsoft.AspNetCore.Identity;

namespace DeliveryAgreagatorApplication.Auth.DAL.Models
{
    public class ApplicationUser: IdentityUser<Guid>
    {

        public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid? CookId { get; set; }
        public Cook? Cook { get; set; }

        public Guid? ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public Guid? CourierId { get; set; }
        public Courier? Courier { get; set; }

        public List<RefreshTokenDb> RefreshTokenDb { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
