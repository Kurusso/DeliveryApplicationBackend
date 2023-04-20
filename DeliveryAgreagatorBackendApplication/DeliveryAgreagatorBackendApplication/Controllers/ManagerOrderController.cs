using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorApplication.Main.Controllers
{
    [Route("api/manager/order")]
    [ApiController]
    public class ManagerOrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public ManagerOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// Получить все заказы ресторана
        /// </summary>
        /// <remarks>
        /// Поле "number" может содержать часть номера искомых заказов. 
        /// Поля startDateOrder, endDateOrder, startDateDelivery и endDateDelivery включают в себя крайние границы. 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet("manager")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int page, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery, int? number = null)
        {
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                var orders = await _orderService.GetRestaurantOrders(page, managerId, startDateOrder, endDateOrder, startDateDelivery, endDateDelivery, number);
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
    }
}
