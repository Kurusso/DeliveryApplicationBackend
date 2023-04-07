using DeliveryAgreagatorBackendApplication.Auth.Models;
using DeliveryAgreagatorBackendApplication.Auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorBackendApplication.Auth.Controllers
{
    [Route("api/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var tokenPair = await _authenticationService.Register(model);
                return Ok(tokenPair);

            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var tokenPair = await _authenticationService.Login(model);
                return Ok(tokenPair);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        [HttpGet("refresh")]
        [Authorize(Policy ="RefreshOnly", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Refresh()
        {
            try
            {
               var tokenPair = await _authenticationService.Refresh(User);
                return Ok(tokenPair);
            }
            catch(Exception ex)
            {
                return Problem("Not Imlemented", statusCode: 501);
            }

        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> test()
        {
            return Ok(User.FindFirst("IdClaim").Value);
        }
    }
}
