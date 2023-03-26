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
    }
}
