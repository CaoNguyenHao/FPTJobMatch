using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FPTJobMatch.Areas.Admin.Models
{
    public class DeleteModel : RolePageModel
    {

        public DeleteModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {

        }

        public IdentityRole role { get; set; }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }


        public async Task<IActionResult> OnGet(string roleid)
        {
            if (roleid == null) return NotFound("Not Found");

            role = await _roleManager.FindByIdAsync(roleid);

            if (role == null)
            {
                return NotFound("Not Found");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("Not Found");

            role = await _roleManager.FindByIdAsync(roleid);

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Delete successfully: {role.Name}";
                return RedirectToPage("./Index");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }

            return Page();
        }
    }
}