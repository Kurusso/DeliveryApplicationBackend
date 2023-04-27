using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorApplication.AdminPanel.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService) 
        {
            _restaurantService = restaurantService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var restaurants = await _restaurantService.GetRestaurants();
                return View(new RestaurantViewModel {Get = restaurants });
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode:501);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post(RestaurantViewModel model)
        {
            Console.WriteLine("fwfw");
            return Ok();
        }
    }
}
