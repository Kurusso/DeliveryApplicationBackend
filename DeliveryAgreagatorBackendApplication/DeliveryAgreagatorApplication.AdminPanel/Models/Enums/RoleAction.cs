
using Microsoft.OpenApi.Attributes;

namespace DeliveryAgreagatorApplication.AdminPanel.Models.Enums
{
    public enum RoleAction
    {
        [Display(name: "Выдать")]
        Give,
        [Display(name: "Забрать")]
        Retrive,
        [Display(name: "Забанить")]
        Ban
    }
}
