﻿namespace DeliveryAgreagatorBackendApplication.Models
{
    public class DishInCartDbModel
    {
        public Guid Id { get; set; }

        public Guid DishId { get; set; }
        public DishDbModel Dish { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerDbModel Customer { get; set; }

        public Guid? OrderId { get; set; }
        public OrderDbModel? Order { get; set; }
        public bool Active { get; set; }
        public int Counter { get; set; }
    }
}
