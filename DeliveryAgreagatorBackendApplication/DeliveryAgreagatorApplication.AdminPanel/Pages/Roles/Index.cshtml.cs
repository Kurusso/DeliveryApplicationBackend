using DeliveryAgreagatorApplication.AdminPanel.Models.DTO;
using DeliveryAgreagatorApplication.AdminPanel.Models.Enums;
using DeliveryAgreagatorApplication.AdminPanel.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryAgreagatorApplication.AdminPanel.Pages.Roles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IRoleService _roleService;

        public IndexModel(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [BindProperty]
        public SetRoleDTO RoleModel { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync() 
        {
            try
            {
                await _roleService.EditUserRoles(RoleModel);
                TempData["SuccessMessage"] = "Изменения выполнены!";
                return RedirectToPage();
            }
            catch(Exception ex)
            {
                TempData["SomethingWrongMessage"] = ex.Message;
                return RedirectToPage(); 
            }
        }
    }
}
