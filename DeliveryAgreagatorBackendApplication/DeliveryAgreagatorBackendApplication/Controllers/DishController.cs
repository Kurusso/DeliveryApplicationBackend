using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        /// <summary>
        /// Получить информацию о блюде
        /// </summary>
        /// <returns></returns>
        /// /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid restaurantId, Guid Id)
        {
            try
            {
                var dish = await _dishService.GetDish(restaurantId, Id);
                return Ok(dish);
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
        /// <summary>
        /// Получить информацию о блюде
        /// </summary>
        /// <returns></returns>
        /// /// <response code="200">Success</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("{id}/rating")]
        [Authorize(Policy = "SetRating", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SetRating(Guid restaurantId, Guid Id, int rating)  //TODO: заменить получение userID из запроса, на получение из токена. 
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _dishService.SetRating( restaurantId, Id, userId,rating);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return Problem(title: e.Message, statusCode: 400);
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
    }
}
