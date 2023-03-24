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
                throw new NotImplementedException();
            }
            catch(NotImplementedException)
            {
                return Problem(title: "Not implemented", statusCode: 501);
            }
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            {
                return Problem(title: "Not implemented", statusCode: 501);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (NotImplementedException)
            {
                return Problem(title: "Not implemented", statusCode: 501);
            }
        }
    }
}
