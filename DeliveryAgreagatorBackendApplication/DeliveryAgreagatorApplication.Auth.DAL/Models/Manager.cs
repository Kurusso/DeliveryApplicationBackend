﻿namespace DeliveryAgreagatorApplication.Auth.DAL.Models
{
    public class Manager
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
