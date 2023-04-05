﻿using DeliveryAgreagatorBackendApplication.Models.DTO;
using DeliveryAgreagatorBackendApplication.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurant/{restaurantId}/dish/")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }
        /// <summary>
        /// Получить информацию о блюде
        /// </summary>
        /// <returns></returns>
        /// /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Блюда с введённым id не существует в ресторане с restaurantId</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DishDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(Guid restaurantId, Guid Id)
        {
            try
            {
                var dish = await _dishService.GetDish(restaurantId, Id);
                return Ok(dish);
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
        /// <summary>
        /// Получить информацию о блюде
        /// </summary>
        /// <returns></returns>
        /// /// <response code="200">Успешное выполнение</response>
        /// <response code="401">Нельзя оценивать блюда, которые не разу не были заказаны!</response>
        /// <response code="404">Блюда с введённым id не существует в ресторане с restaurantId</response>
        [HttpPost("{id}/rating")]
        public async Task<IActionResult> SetRating(Guid restaurantId, Guid Id, Guid userId, int rating)  //TODO: заменить получение userID из запроса, на получение из токена. 
        {
            try
            {
                await _dishService.SetRating( restaurantId, Id, userId,rating);
                return Ok();
            }
            catch (ArgumentNullException e)
            {
                return Problem(title: e.Message, statusCode: 401);
            }
            catch (ArgumentException e)
            {
                return Problem(title: e.Message, statusCode: 404);
            }
        }
    }
}
