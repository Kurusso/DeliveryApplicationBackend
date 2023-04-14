using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.API.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Exceptions;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.Net;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurant/{restaurantId}/menu/")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService) 
        {
        _menuService = menuService;
        }
        /// <summary>
        /// Получить список меню ресторана
        /// </summary>
        /// <remarks>
        /// При значении active = false : метод вернёт все меню, а не только неактивные.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not Found</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MenuShortDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenus(Guid restaurantId, bool active) 
        {
            try {
               var menus = await _menuService.GetRestaurantMenus(restaurantId, active);
                return Ok(menus);
            }
            catch(WrongIdException ex) {
                return Problem(ex.Message, statusCode:ex.StatusCode);
			}
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        /// <summary>
        /// Получить блюда в меню
        /// </summary>
        /// <remarks>
        /// При значении isVegetarian = false : метод вернёт все блюда, а не только не вегетарианские.
        /// </remarks>
        /// <returns></returns>
        ///  <response code="200">Success</response>
        ///  <response code="404">Not Found</response>
        /// <response code="400">Bad Request</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<DishDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenu(Guid restaurantId, Guid id, [FromQuery] List<Category> categories, bool? isVegetarian, DishFilter? filter, int page) {
            try {
                var dishes = await _menuService.GetMenuDishes(restaurantId, id, categories, isVegetarian, filter, page);
                return Ok(dishes);
			}
			catch (ArgumentOutOfRangeException ex)
			{
				return Problem(title: ex.Message, statusCode: 400);
			}
            catch(WrongIdException ex) {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }

    }
}
