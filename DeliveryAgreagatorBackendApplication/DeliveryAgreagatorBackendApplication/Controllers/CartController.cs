using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/cart/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        /// <summary>
        /// Получить блюда в корзине
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize(Policy = "CartOperations", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<DishInCartDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get() 
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                var dsihes = await _cartService.GetCart(userId);
                return Ok(dsihes);
            }
            catch(Exception e)
            {
                return Problem("Not implemented", statusCode: 501);
            }
        }
        /// <summary>
        /// Добавить блюдо в корзину
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("dish/{dishId}")]
        [Authorize(Policy = "CartOperations", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostToCart(Guid dishId) 
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _cartService.AddDishToCart(dishId, userId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return Problem(title:e.Message, statusCode: 404);
            }
        }
        /// <summary>
        /// Удалить блюдо из корзины
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete("dish/{dishId}")]
        [Authorize(Policy = "CartOperations", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteDecrease( Guid dishId, bool deacrease=false) 
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value,out userId);
                await _cartService.DeleteOrDecreaseDishInCart( dishId, userId , deacrease);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
    }
}
