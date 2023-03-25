using DeliveryAgreagatorBackendApplication.Models.Enums;
using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

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
        [HttpGet]
        public async Task<IActionResult> GetMenus(Guid restaurantId, bool active) 
        {
            try {
               var menus = await _menuService.GetRestaurantMenus(restaurantId, active);
                return Ok(menus);
            }
            catch(ArgumentException e) {
                return Problem(title:e.Message, statusCode:401);
			}
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenu(Guid restaurantId, Guid id, [FromQuery] Category[] categories, bool isVegetarian, DishFilter filter, int page) {
            try {
				throw new NotImplementedException();
			}
			catch (NotImplementedException e)
			{
				return Problem(title: "Not implemented", statusCode: 501);
			}
		}

    }
}
