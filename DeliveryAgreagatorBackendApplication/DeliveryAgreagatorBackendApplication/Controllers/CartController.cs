using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/cart/")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get(Guid userId) //TODO: заменить получение id из запроса, на получение из токена
        {
            try
            {

            }
            catch
            {
            
            }
        }
    }
}
