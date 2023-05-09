using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
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
        private readonly IRoleService _roleService;
        public IndexModel(IRestaurantService restaurantService, IRoleService roleService)
        {
            _restaurantService = restaurantService;
            _roleService = roleService;

        }

        [BindProperty]
        public RestaurantShortDTO Restaurant { get; set; }

        [BindProperty]
        public List<UserDTO> Managers { get; set; }

        [BindProperty]
        public List<UserDTO> Cooks { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                Restaurant = await _restaurantService.GetRestaurantById((Guid)id);
                Managers = await _roleService.GetUsersWithRole(id, Role.Manager);
                Cooks = await _roleService.GetUsersWithRole(id, Role.Cook);
                return Page();
            }
            catch (WrongIdException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if(!ModelState.IsValid)
            {
                Managers = await _roleService.GetUsersWithRole(id, Role.Manager);
                Cooks = await _roleService.GetUsersWithRole(id, Role.Cook);
                return Page();
            }
            try
            {
                await _restaurantService.PutRestaurant(Restaurant);
                TempData["SuccessMessage"] = "Изменения выполнены!";
                return RedirectToPage();
            }
            catch (WrongIdException ex)
            {
                TempData["SomethingWrongMessage"] = ex.Message;
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
