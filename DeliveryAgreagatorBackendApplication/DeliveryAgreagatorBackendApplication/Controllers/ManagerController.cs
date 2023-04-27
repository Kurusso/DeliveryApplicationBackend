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
    [Route("api/manager/")]
    [ApiController]
    public class ManagerController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private readonly IDishService _dishService;
        public ManagerController(IOrderService orderService, IMenuService menuService, IDishService dishService)
        {
            _orderService = orderService;
            _menuService = menuService;
            _dishService = dishService;
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
        [HttpGet("order")]
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

        [HttpPost("menu")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PostMenu(MenuDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                await _menuService.AddMenu(model, managerId);
                return Ok();
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

        [HttpPut("menu/{id}")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> ChangeMenu(Guid id, MenuDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                await _menuService.EditMenu(id, model, managerId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 403);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

        [HttpPost("menu/{menuId}")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateDish(Guid menuId, DishPostDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                await _dishService.CreateDish(managerId, menuId, model);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 403);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

        [HttpPut("menu/{menuId}/dish/{dishId}")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddDish(Guid menuId, Guid dishId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                await _dishService.AddDishToMenu(managerId,menuId, dishId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 403);
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

        [HttpPut("dish/{dishId}")]
        [Authorize(Policy = "OrderOperationsManager", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> EditDish(Guid dishId, DishPutDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid managerId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out managerId);
                await _dishService.EditDish(managerId, dishId, model);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message, statusCode: 403);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
    }
}
