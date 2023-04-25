using DeliveryAgreagatorApplication.Common.Models.Enums;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorApplication.Main.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IRabbitMqService _mqService;

        public TestController(IRabbitMqService mqService)
        {
            _mqService = mqService;
        }
        [HttpGet]
        public async Task<IActionResult> SendMessage(OrderStatus message)
        {
           await _mqService.SendMessage(new Notification(new Guid(), 8787, message));

            return Ok("Сообщение отправлено");
        }
    }
}
