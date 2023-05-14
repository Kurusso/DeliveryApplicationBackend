using Microsoft.OpenApi.Attributes;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.Enums
{
    public enum Role
    {
        [Display(name: "Менеджер")]
        Manager,
        [Display(name: "Повар")]
        Cook,
        [Display(name: "Курьер")]
        Courier
    }
}
