namespace DeliveryAgreagatorBackendApplication.Models.DTO
{
    public class RestaurantShortDTO
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Picture { get; set; }
		public RestaurantShortDTO(RestaurantDbModel restaurantDb) 
		{ 
		Id= restaurantDb.Id;
		Name= restaurantDb.Name;
	    Picture= restaurantDb.Picture;
		}
	}
}
