using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/order/")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid userId, OrderPostDTO model) //TODO: заменить получение id из запроса, на получение из токена
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            try
            {
              await  _orderService.PostOrder(model, userId);
                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 401);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id, Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                await _orderService.CancelOrder(id, userId);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
            catch(InvalidOperationException ex) 
            {
                return Problem(title: ex.Message, statusCode: 401);
            }
        }
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentOrders(Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
               var orders = await _orderService.GetActiveOrders(userId);
                return Ok(orders);
            }
            catch (Exception ex) 
            {
                return Problem("Unexpected exception!", statusCode: 501);
            }
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders(int page, Guid userId, bool? filterDateAsc = null, string? name = null)
        {
            try
            {
               var orders = await _orderService.GetAllOrders(page, userId, filterDateAsc, name);
                return Ok(orders);
            }
            catch(ArgumentOutOfRangeException ex) 
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
        }
        [HttpPost("{id}/repeat")]
        public async Task<IActionResult> RepeatOrder(Guid id, Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                await _orderService.RepeatOrder(id, userId);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 401);
            }
        }
    }
}
