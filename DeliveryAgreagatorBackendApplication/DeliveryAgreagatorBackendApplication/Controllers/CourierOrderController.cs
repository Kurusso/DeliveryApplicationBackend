using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorApplication.Main.Controllers
{
    [Route("api/courier/order/")]
    [ApiController]
    public class CourierOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public CourierOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Получить заказы достпупные курьеру
        /// </summary>
        /// <remarks>
        /// Поле блюд внутри заказа приходит, пустым так как это излишняя информация для курьера.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int page)
        {
            try
            {
                Guid courierId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out courierId);
                var orders = await _orderService.GetOrdersAvaliableToCourier(courierId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Взять/Обновить статус заказа
        /// </summary>
        /// <remarks>
        /// При значении поля take=true, метод назанчит заказ курьеру, а при take=false изменит стадию доставки на следующую.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpPut("{id}")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutCourier(Guid id, StatusDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid courierId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out courierId);
                await _orderService.TakeOrderCourier(id, courierId, model);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Отменить заказ
        /// </summary>
        /// <remarks>
        /// Курьер может отменить только тот заказ, котороый находиться в статусе "Delivery".
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpDelete("{id}/cancel")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CancelCourier(Guid id)
        {
            try
            {
                Guid courierId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out courierId);
                await _orderService.CancelOrderCourier(id, courierId);
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
