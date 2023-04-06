using DeliveryAgreagatorBackendApplication.Model.Enums;
using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
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
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Bad Request</response>
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
        /// <summary>
        /// Удалить заказ
        /// </summary>
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Bad Request</response>
        /// <response code="404">Not Found</response>
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
        /// <summary>
        /// Получить все свои заказы
        /// </summary>
        /// <remarks>
        /// Поле userId временное, будет убрано после добавления авторизации и аутентификации. 
        /// Поле "name" может содержать часть номера искомых заказов. 
        /// Поля startDate и endDate включают в себя крайние границы. 
        /// Поле active - показывает только актвные в случае active=true и только историю заказав при active=false.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{active}")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllOrders(bool active,int page, Guid userId, DateTime startDate, DateTime endDate, int? number = null)
        {
            try
            {
               var orders = await _orderService.GetAllOrders(page, userId, startDate, endDate,active, number);
                return Ok(orders);
            }
            catch(ArgumentOutOfRangeException ex) 
            {
                return Problem(title: ex.Message, statusCode: 404);
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
        /// <response code="401">Bad Request</response>
        /// <response code="404">Not Found</response>
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

        /// <summary>
        /// Получить доступные повару заказы
        /// </summary>
        /// <remarks>
        /// Поле cookId временное, будет убрано после добавления авторизации и аутентификации. 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet("cook/active")]
        public async Task<IActionResult> Get(int page, Guid cookId, DateSort? sort = null) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {
                var orders = await _orderService.GetOrdersAvaliableToCook(sort, page, cookId); 
                return Ok(orders);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Взять/Выполнить заказ поваром
        /// </summary>
        /// <remarks>
        /// Поле cookId временное, будет убрано после добавления авторизации и аутентификации. 
        /// При значении поля take=true, метод назанчит заказ повару, а при take=false изменит стадию приготовления на следующую.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        [HttpPut("{id}/cook/{take}")]
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
        /// <summary>
        /// История заказов повара
        /// </summary>
        /// <remarks>
        /// Поле cookId временное, будет убрано после добавления авторизации и аутентификации. 
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="500">Not Implemented</response>
        [HttpGet("cook/done")]
        [ProducesResponseType(typeof(List<OrderDTO>), (int)HttpStatusCode.OK)]
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
        public async Task<IActionResult> PutCourier(Guid id, bool take, Guid courierId)
        {
            try
            {
                await _orderService.TakeOrderCourier(id,take, courierId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 401);
            }
        }
        [HttpPut("{id}/courier/cancel")]
        public async Task<IActionResult> CancelCourier(Guid id, Guid courierId)
        {
            try
            {
                await _orderService.CancelOrderCourier(id, courierId);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(title: ex.Message, statusCode: 401);
            }
            catch(ArgumentException ex)
            {
                return Problem(title: ex.Message, statusCode: 404);
            }
        }
        [HttpGet("manager")]
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
