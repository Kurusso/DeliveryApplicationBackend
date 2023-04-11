using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurants/")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;
        public RestaurantController(IRestaurantService restaurantService) 
        {
        _restaurantService = restaurantService;
        }
        /// <summary>
        /// Получение списка ресторанов
        /// </summary>
        /// <remarks>
        /// Поле "name" может содержать часть искомого имени ресторана 
        /// </remarks>
        /// <returns></returns>
        /// /// <response code="200">Success</response>
        /// <response code="401">Bad Request</response>
        [HttpGet("{page}")]
        [ProducesResponseType(typeof(List<RestaurantShortDTO>), (int)HttpStatusCode.OK)]
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
