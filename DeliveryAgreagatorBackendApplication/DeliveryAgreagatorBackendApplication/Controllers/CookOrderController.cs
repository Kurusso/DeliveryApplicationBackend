using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorApplication.Main.Controllers
{
    [Route("api/cook/order/")]
    [ApiController]
    public class CookOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public CookOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Получить заказы повора
        /// </summary>
        /// <remarks>
        /// Поле active - показывает актвные и возможные в случае active=true и только историю заказав при active=false.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{active}")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(bool active, int page, DateSort? sort = null)
        {
            try
            {
                Guid cookId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out cookId);
                var orders = await _orderService.GetOrdersAvaliableToCook(active, sort, page, cookId);
                return Ok(orders);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Problem(ex.Message, statusCode: 404);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Взять/Выполнить заказ поваром
        /// </summary>
        /// <remarks>
        /// При значении поля take=true, метод назанчит заказ повару, а при take=false изменит стадию приготовления на следующую.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put(Guid id, StatusDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid cookId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out cookId);
                await _orderService.TakeOrderCook(id, cookId, model);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 404);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

    }
}
