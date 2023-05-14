using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;

namespace DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<LoginStatusDTO> VerifyLogin(LoginDTO loginModel);
    }
}
