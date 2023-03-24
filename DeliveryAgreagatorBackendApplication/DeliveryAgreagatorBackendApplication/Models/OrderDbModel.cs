﻿using DeliveryAgreagatorBackendApplication.Models.Enums;

namespace DeliveryAgreagatorBackendApplication.Models
{
    public class OrderDbModel
    {
        public Guid Id { get; set; }
        public DateTime DeliveryTime { get; set; }
        public DateTime OrderTime { get; set; }
        public int Price { get; set; } 
        public string Address { get; set; }
        public Status Status { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerDbModel Customer { get; set; }

        public Guid? CourierId { get; set; }
        public CourierDbModel? Courier { get; set; }

        public Guid? CookId { get; set; }
        public CookDbModel? Cook { get; set; }

        public ICollection<DishInCartDbModel> DishesInCart { get; set;}
    }
}
