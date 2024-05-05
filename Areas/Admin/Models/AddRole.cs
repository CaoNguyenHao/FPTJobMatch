using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FPTJobMatch.Areas.Admin.Models
{
    public class AddRoleModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddRoleModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public IdentityUser user { get; set; }

        [BindProperty]
        [DisplayName("Roles assigned to users")]

        public string[] RoleNames { get; set; }

        public SelectList allRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("No User");
            }
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User Not Found, id = {id}.");
            }

            RoleNames = (await _userManager.GetRolesAsync(user)).ToArray<string>();
            List<string> roleName = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            allRoles = new SelectList(roleName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound("No User");
            }

            user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound($"User Not Found, id = {id}.");
            }

            var OldRoleNames = (await _userManager.GetRolesAsync(user)).ToArray();
            var deleteRoles = OldRoleNames.Where(r => !RoleNames.Contains(r));

            var addRoles = RoleNames.Where(r => !OldRoleNames.Contains(r));

            List<string> roleName = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            allRoles = new SelectList(roleName);

            var resultDelete = await _userManager.RemoveFromRolesAsync(user, deleteRoles);
            if (!resultDelete.Succeeded)
            {
                resultDelete.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            if (!resultAdd.Succeeded)
            {
                resultAdd.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
                return Page();
            }
            StatusMessage = $"Update Role cho User: {user.UserName}";
            return RedirectToPage("./Index");
        }


    }

}
