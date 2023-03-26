using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurant/{restaurantId}/dish/")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid restaurantId, Guid Id) 
        {
            try 
            {
               var dish = await _dishService.GetDish(restaurantId, Id);
                return Ok(dish);
            }
            catch(ArgumentException e) 
            {
                return Problem(title:e.Message, statusCode: 404);
            }
        }
    }
}
