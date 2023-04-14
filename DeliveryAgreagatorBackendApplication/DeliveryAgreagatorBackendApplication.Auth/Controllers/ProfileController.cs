using DeliveryAgreagatorApplication.Auth.Common.Interfaces;
using DeliveryAgreagatorApplication.Auth.Common.Models;
using DeliveryAgreagatorApplication.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryAgreagatorApplication.Auth.Controllers
{
    [Route("api/profile/")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get()
        {
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                var profile = await _profileService.GetProfile(userId);
                return Ok(profile);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Put(ProfileDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _profileService.ChangeProfile(userId, model);
                return Ok();
            }
            catch (ConflictException ex)
            {
                return Problem(ex.Message, statusCode: ex.StatusCode);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, statusCode: 501);
            }
        }
        [HttpPut("password")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> PutPassword(PasswordUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Guid userId;
                Guid.TryParse(User.FindFirst("IdClaim").Value, out userId);
                await _profileService.UpdatePassword(userId, model);
                return Ok();
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
    }

}
