using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorApplication.Main.Controllers
{
    [Route("api/customer/order/")]
    [ApiController]
    public class CustomerOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public CustomerOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Создать заказ
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("post")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Post(OrderPostDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _orderService.PostOrder(model, User);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Удалить заказ
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpDelete("{id}")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _orderService.CancelOrder(id, userId);
                return Ok();
            }
            catch (WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
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
        /// Получить все свои заказы
        /// </summary>
        /// <remarks>
        /// Поле "number" может содержать часть номера искомых заказов. 
        /// Поля startDate и endDate включают в себя крайние границы. 
        /// Поле active - показывает только актвные в случае active=true и только историю заказав при active=false.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{active}")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllOrders(bool active, int page, DateTime startDate, DateTime endDate, int? number = null)
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                var orders = await _orderService.GetAllOrders(page, userId, startDate, endDate, active, number);
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
        /// Повторить заказ
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpPost("{id}/repeat")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RepeatOrder(Guid id)
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _orderService.RepeatOrder(id, userId);
                return Ok();
            }
            catch (WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
    }
}
