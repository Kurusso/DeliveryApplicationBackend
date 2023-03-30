namespace DeliveryAgreagatorBackendApplication.Models.DTO
{
    public class MenuShortDTO
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public bool IsActive { get; set; }
		public MenuShortDTO(MenuDbModel model){
		Id= model.Id;
		Name= model.Name;
		IsActive= model.isActive;
		}
	}
}
