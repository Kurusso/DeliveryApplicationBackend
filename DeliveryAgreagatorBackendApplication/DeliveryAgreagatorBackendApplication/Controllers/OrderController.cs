using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

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
        /// <summary>
        /// Создать заказ
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
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
            catch(WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch(InvalidOperationException ex) 
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
        /// Поле "name" может содержать часть номера искомых заказов. 
        /// Поля startDate и endDate включают в себя крайние границы. 
        /// Поле active - показывает только актвные в случае active=true и только историю заказав при active=false.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{active}/customer")]
        [Authorize(Policy = "OrderOperationsCustomer", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
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
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации. 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
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
            catch(WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

        /// <summary>
        /// Получить доступные повару заказы
        /// </summary>
        /// <remarks>
        /// Поле cookId временное, будет убрано после добавления авторизации и аутентификации. 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("cook/active")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int page, DateSort? sort = null) 
        {
            try
            {
                Guid cookId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out cookId);
                var orders = await _orderService.GetOrdersAvaliableToCook(sort, page, cookId); 
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
        [HttpPut("{id}/cook/{take}")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put(Guid id, bool take)
        {
            try
            {
                Guid cookId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out cookId);
                await _orderService.TakeOrderCook(id, take ,cookId);
                return Ok();
            }
            catch(InvalidOperationException ex)
            {
                return Problem(title:ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// История заказов повара
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("cook/done")]
        [Authorize(Policy = "OrderOperationsCook", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int page, int? number=null) 
        {
            try
            {
                Guid cookId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out cookId);
                var orders = await _orderService.GetCookOrdersStory(number,page,cookId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
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
        [HttpGet("courier")]
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
        [HttpPut("{id}/courier/{take}")]
        [Authorize(Policy = "OrderOperationsCourier", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutCourier(Guid id, bool take)
        {
            try
            {
                Guid courierId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out courierId);
                await _orderService.TakeOrderCourier(id,take, courierId);
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
        [HttpDelete("{id}/courier/cancel")]
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
            catch(WrongIdException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Получить все заказы ресторана
        /// </summary>
        /// <remarks>
        /// Поле "name" может содержать часть номера искомых заказов. 
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
        public async Task<IActionResult> Get(int page, DateTime startDateOrder, DateTime endDateOrder, DateTime startDateDelivery, DateTime endDateDelivery,  int? number = null)
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
                return Problem(ex.Message, statusCode:404);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
    }
}
