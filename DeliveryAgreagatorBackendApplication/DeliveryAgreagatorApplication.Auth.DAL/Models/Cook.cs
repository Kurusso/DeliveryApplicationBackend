﻿namespace DeliveryAgreagatorApplication.Auth.DAL.Models
{
    public class Cook
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
