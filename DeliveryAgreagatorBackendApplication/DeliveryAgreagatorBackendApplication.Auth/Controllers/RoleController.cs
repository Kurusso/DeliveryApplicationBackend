using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeliveryAgreagatorBackendApplication.Auth.Controllers
{
    [Route("api/role/")]
    [ApiController]
    public class RoleController : ControllerBase
        {
            private RoleManager<IdentityRole<Guid>> roleManager;
            public RoleController(RoleManager<IdentityRole<Guid>> roleMgr)
            {
                roleManager = roleMgr;
            }


            private void Errors(IdentityResult result)
            {
                foreach (IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }

        [HttpPost("addRole")]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole<Guid>(name));
            }
            return Ok();
        }
    }

}
