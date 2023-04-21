using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Exceptions;
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
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid restaurantId, Guid id)
        {
            try
            {
                var dish = await _dishService.GetDish(restaurantId, id);
                return Ok(dish);
            }
            catch (WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Получить информацию о блюде
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="403">Forbidden</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="501">Not Implemented</response>
        [HttpPost("{id}/rating")]
        [Authorize(Policy = "SetRating", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> SetRating(Guid restaurantId, Guid id, int rating) 
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _dishService.SetRating( restaurantId, id, userId,rating);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
            catch (WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
    }
}
