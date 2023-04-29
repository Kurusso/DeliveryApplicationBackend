using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
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
        private readonly UserManager<ApplicationUser> _userManager; 
        public IndexModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                var user = await _userManager.FindByEmailAsync(Input.Email);

                var passwordVerified = BCrypt.Net.BCrypt.Verify(Input.Password, user.PasswordHash);

                if (passwordVerified)
                {
                    // Sign in the user and create a cookie
                    await _signInManager.SignInAsync(user, Input.RememberMe);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
