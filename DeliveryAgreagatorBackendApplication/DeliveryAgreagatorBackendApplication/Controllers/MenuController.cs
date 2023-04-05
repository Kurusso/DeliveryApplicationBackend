using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Models.Enums;
using DeliveryAgreagatorBackendApplication.Services;
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
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Не существует ресторана restaurantId</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<MenuShortDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenus(Guid restaurantId, bool active) 
        {
            try {
               var menus = await _menuService.GetRestaurantMenus(restaurantId, active);
                return Ok(menus);
            }
            catch(ArgumentException e) {
                return Problem(title:e.Message, statusCode:404);
			}
        }
        /// <summary>
        /// Получить блюда в меню
        /// </summary>
        /// <remarks>
        /// При значении isVegetarian = false : метод вернёт все блюда, а не только не вегетарианские.
        /// </remarks>
        /// <returns></returns>
        ///  <response code="200">Успешное выполнение</response>
        ///  <response code="404">Не существует меню id в ресторане restaurantId</response>
        /// <response code="401">Указан неверный номер страницы</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<DishDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenu(Guid restaurantId, Guid id, [FromQuery] List<Category> categories, bool? isVegetarian, DishFilter? filter, int page) {
            try {
                var dishes = await _menuService.GetMenuDishes(restaurantId, id, categories, isVegetarian, filter, page);
                return Ok(dishes);
			}
			catch (ArgumentOutOfRangeException e)
			{
				return Problem(title: e.Message, statusCode: 401);
			}
            catch(ArgumentException e) {
                return Problem(title: e.Message, statusCode: 404);
            }
		}

    }
}
