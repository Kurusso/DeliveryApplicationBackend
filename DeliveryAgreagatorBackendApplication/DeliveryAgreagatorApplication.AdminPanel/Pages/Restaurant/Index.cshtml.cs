using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryAgreagatorApplication.AdminPanel.Pages.Restaurant
{
    public class IndexModel : PageModel
    {
        private readonly IRestaurantService _restaurantService;

        [BindProperty]
        public RestaurantViewModel? Restaurant { get; set; }

        public IndexModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var restaurants = await _restaurantService.GetRestaurants();
            Restaurant = new RestaurantViewModel { Get = restaurants };
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _restaurantService.PostRestaurant(Restaurant.Post);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            try
            {
                await _restaurantService.DeleteRestaurant(id);
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
