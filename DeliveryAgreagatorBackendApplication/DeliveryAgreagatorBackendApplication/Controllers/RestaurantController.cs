using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Controllers
{
    [Route("api/backend/restaurant/")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(Guid Id) 
        {
            try
            {
                throw
            }
            catch
            {

            }
        }
    }
}
