using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> Get(Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                var dsihes = await _cartService.GetCart(userId);
                return Ok(dsihes);
            }
            catch(Exception e)
            {
                return Problem("Not implemented", statusCode: 501);
            }
        }
        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> PostToCart(Guid dishId,Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                await _cartService.AddDishToCart( dishId, userId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return Problem(title:e.Message, statusCode: 404);
            }
        }
        [HttpDelete("dish/{dishId}")]
        public async Task<IActionResult> DeleteDecrease( Guid dishId, Guid userId, bool deacrease=false) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                await _cartService.DeleteOrDecreaseDishInCart( dishId, userId, deacrease);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
    }
}
