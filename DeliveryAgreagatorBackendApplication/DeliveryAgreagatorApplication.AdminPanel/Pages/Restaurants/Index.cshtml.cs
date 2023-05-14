using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

namespace DeliveryAgreagatorApplication.AdminPanel.Pages.Restaurants
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IRestaurantAdminService _restaurantAdminService;

        [BindProperty]
        public RestaurantViewModel? Restaurant { get; set; }


        [BindProperty]
        public int PageNo { get; set; }

        [BindProperty]
        public int TotalRecords { get; set; }

        [BindProperty]
        public string? RestaurantName { get; set; }

        [BindProperty]
        public int PageSize { get; set; }

        public IndexModel(IRestaurantService restaurantService, IRestaurantAdminService restaurantAdminService)
        {
            _restaurantService = restaurantService;
            _restaurantAdminService = restaurantAdminService;
        }
        [HttpGet]
        public async Task<IActionResult> OnGetAsync(int p=1, int s=5, string? name = null)
        {
            var reserved = RestaurantName;
            RestaurantName = name==null ? Request.Query["RestaurantName"] : name;
            if (RestaurantName == null)
            {
                RestaurantName = reserved;
            }
            var restaurants = await _restaurantService.GetRestaurants(p, RestaurantName, s);
            TotalRecords = await _restaurantAdminService.GetRestaurantsCount(RestaurantName);
            Restaurant = new RestaurantViewModel { Get = restaurants };
            PageNo = p;
            PageSize = s;
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync(int p = 1, int s = 5, string? name = null)
        {

            if (!ModelState.IsValid)
            {

                TotalRecords = await _restaurantAdminService.GetRestaurantsCount(RestaurantName);
                Restaurant.Get = await _restaurantService.GetRestaurants(p, RestaurantName, s);
                return Page();
            }
            try
            {
                string? restaurantName = Request.Form["RestaurantName"];
                await _restaurantAdminService.PostRestaurant(Restaurant.Post);
                TempData["SuccessMessage"] = "Ресторан Добавлен!";
                return RedirectToPage("/Restaurants/Index", new { p = p, s = s, name = name });
            }
            catch (Exception ex)
            {
                TempData["SomethingWrongMessage"] = ex.Message;
                return RedirectToPage();
            }
        }
        [HttpDelete]
        public async Task<IActionResult> OnPostDeleteAsync(Guid id, int p = 1, int s = 5, string? name=null)
        {
            try
            {
                await _restaurantAdminService.DeleteRestaurant(id);
                return RedirectToPage("/Restaurants/Index", new { p = p, s =s, name=name});
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }
}
