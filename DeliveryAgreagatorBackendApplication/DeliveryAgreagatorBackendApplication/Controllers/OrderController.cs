using DeliveryAgreagatorBackendApplication.Model.Enums;
using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Post(OrderPostDTO model) 
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await  _orderService.PostOrder(model, userId);
                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }
        [HttpDelete("{id}/customer")]
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
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
            catch(InvalidOperationException ex) 
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }

        [HttpGet("{active}/customer")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetAllOrders(bool active,int page, DateTime startDate, DateTime endDate, int? number = null)
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                var orders = await _orderService.GetAllOrders(page, userId, startDate, endDate,active, number);
                return Ok(orders);
            }
            catch(ArgumentOutOfRangeException ex) 
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
        }
        [HttpPost("{id}/repeat/customer")]
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
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }
        [HttpGet("cook/active")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int page, Guid cookId, DateSort? sort = null) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                var orders = await _orderService.GetOrdersAvaliableToCook(sort, page, cookId); 
                return Ok(orders);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        [HttpPut("{id}/cook/{take}")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put(Guid id, bool take, Guid cookId)
        {
            try
            {
                await _orderService.TakeOrderCook(id, take ,cookId);
                return Ok();
            }
            catch(ArgumentException ex)
            {
                return Problem(title:ex.Message, statusCode: 404);
            }
        }

        [HttpGet("cook/done")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int page, Guid cookId, int? number=null) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                var orders = _orderService.GetCookOrdersStory(number,page,cookId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        [HttpGet("courier")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int page, Guid courierId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                var orders = await _orderService.GetOrdersAvaliableToCourier(courierId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 500);
            }
        }
        [HttpPut("{id}/courier/{take}")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutCourier(Guid id, bool take, Guid courierId)
        {
            try
            {
                await _orderService.TakeOrderCourier(id,take, courierId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
        }
        [HttpDelete("{id}/courier/cancel")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CancelCourier(Guid id, Guid courierId)
        {
            try
            {
                await _orderService.CancelOrderCourier(id, courierId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
        }
        [HttpGet("manager")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get(int page, Guid managerId, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery,  int? number = null)
        {
            try
            {
                var orders = await _orderService.GetRestaurantOrders(page, managerId, startDateOrder, endDateOrder, startDateDelivery, endDateDelivery, number);
                return Ok(orders);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return Problem(title: ex.Message, statusCode:404);
            }
        }
    }
}
