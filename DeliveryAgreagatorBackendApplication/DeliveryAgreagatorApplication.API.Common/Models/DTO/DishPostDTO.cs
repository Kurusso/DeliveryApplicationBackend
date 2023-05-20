using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Main.Common.Models.DTO
{
    public class DishPostDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public bool IsVegetarian { get; set; }
        public string PhotoUrl { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Category Category { get; set; }
    }
}
