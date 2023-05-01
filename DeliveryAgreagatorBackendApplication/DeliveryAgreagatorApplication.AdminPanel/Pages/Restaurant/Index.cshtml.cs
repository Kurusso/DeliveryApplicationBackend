using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.API.Common.Models.DTO;
using DeliveryAgreagatorApplication.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryAgreagatorApplication.AdminPanel.Pages.Restaurant
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRestaurantService _restaurantService;

        public IndexModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [BindProperty]
        public RestaurantShortDTO Restaurant { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Restaurant = await _restaurantService.GetRestaurantById((Guid)id);
                return Page();
            }
            catch (WrongIdException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _restaurantService.PutRestaurant(Restaurant);
                return RedirectToPage();
            }
            catch (WrongIdException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
