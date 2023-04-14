using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        /// <summary>
        /// Регистрация
        /// </summary>
        /// <remarks>
        /// Поля login, fullName и phoneNumber должны быть уникальными.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="409">Conflict</response>
        /// <response code="501">Not Implemented</response>
        /// <response code="400">Bad Request</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(TokenPairDTO), (int)HttpStatusCode.OK)]
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
            catch(ConflictException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message,statusCode:501);
            }
        }
        /// <summary>
        /// Логин
        /// </summary>
        /// <remarks>
        /// Поля login, fullName и phoneNumber должны быть уникальными.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="501">Not Implemented</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenPairDTO), (int)HttpStatusCode.OK)]
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
        /// <summary>
        /// Обновление access токена
        /// </summary>
        /// <remarks>
        /// Поля login, fullName и phoneNumber должны быть уникальными.
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="501">Not Implemented</response>
        [HttpGet("refresh")]
        [Authorize(Policy ="RefreshOnly", AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(TokenPairDTO), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Refresh() 
        {
            try
            {
               var tokenPair = await _authenticationService.Refresh(User);
                return Ok(tokenPair);
            }
            catch (TokenException ex)
            {
                return Problem(ex.Message, statusCode:ex.StatusCode);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }

        }
    }
}
