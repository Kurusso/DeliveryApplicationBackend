using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.Auth.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DeliveryAgreagatorApplication.AdminPanel.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILoginService _loginService;
        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILoginService loginService)
        {
            _loginService= loginService;
            _signInManager = signInManager;

        }

        [BindProperty]
        public LoginDTO Input { get; set; }

        public string ReturnUrl { get; set; }


        public async Task<IActionResult> OnGet(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/Index");
            }
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var userVerified = await _loginService.VerifyLogin(Input);

                if (userVerified.IsLogedIn)
                {
                    // Sign in the user and create a cookie
                   await _signInManager.SignInAsync(userVerified.User, Input.RememberMe);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, userVerified.DenyReason);
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
