using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public interface IRoleService
    {
        public Task EditUserRoles(SetRoleDTO model);
    }
}
