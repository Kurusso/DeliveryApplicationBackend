using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorApplication.AdminPanel.Controllers
{
    public class RegistrationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
