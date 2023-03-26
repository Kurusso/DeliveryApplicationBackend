using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurant/")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService) 
        {
        _restaurantService = restaurantService;
        }
        [HttpGet("{Id}/GetById")]
        public async Task<IActionResult> GetById(Guid Id) 
        {
            try
            {
                throw new NotImplementedException();
            }
            catch(NotImplementedException e)
            {
                return Problem(title: "Not implemented", statusCode: 501);
            }
        }

        [HttpGet("{name}/GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException e)
            {
                return Problem(title: "Not implemented", statusCode: 501);
            }
        }

        [HttpGet("{page}/GetRestaurantsOnPage")]
        public async Task<IActionResult> GetAllOnPage(int page, string? name)
        {
            try
            {
                var restaurants = await _restaurantService.GetRestaurants(page, name);
                return Ok(restaurants);
            }
            catch (ArgumentOutOfRangeException e)
            {
                return Problem(title: e.Message, statusCode: 401);
            }
        }
    }
}
