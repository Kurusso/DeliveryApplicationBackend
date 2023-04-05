using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
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
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="501">Не имплементированная ошибка</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<DishInCartDTO>), (int)HttpStatusCode.OK)]
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
        /// <summary>
        /// Добавить блюдо в корзину
        /// </summary>
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Не существует блюда dishId</response>
        [HttpPost("dish/{dishId}")]
        public async Task<IActionResult> PostToCart(Guid dishId,Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
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
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Не существует блюда dishId</response>
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
