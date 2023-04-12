using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
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
            catch(InvalidDataException ex)
            {
                return Problem(ex.Message, statusCode: 409);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message,statusCode:501);
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
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        [HttpGet("refresh")]
        [Authorize(Policy ="RefreshOnly", AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Refresh() //TODO: перенести проверку верности токена в авторизацию
        {
            try
            {
               var tokenPair = await _authenticationService.Refresh(User);
                return Ok(tokenPair);
            }
            catch (ArgumentException ex)
            {
                return Problem(ex.Message, statusCode: 401);
            }
            catch (InvalidOperationException ex)
            {
                return Problem(ex.Message, statusCode: 401);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }

        }
        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> test()
        {
            return Ok(User.FindFirst("GetAllOrders").Value);
        }
    }
}
