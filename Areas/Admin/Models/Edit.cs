using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using FPTJobMatch.Areas.Admin.Models;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FPTJobMatch.Areas.Admin.Models
{
    public class EditModel : RolePageModel
    {
        public EditModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {

        }

        public class InputModel
        {
            [Display(Name = "Name Role")]
            [Required(ErrorMessage = "Must Input{0}}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} is {2} to {1} characters long")]

            public string Name { set; get; }

        }
        [BindProperty]
        public InputModel Input { set; get; }

        public IdentityRole role { get; set; }



        public async Task<IActionResult> OnGet(string roleid)
        {
            //return Page();
            if (roleid == null) return NotFound("Not Found");

            role = await _roleManager.FindByIdAsync(roleid);
            if (role != null)
            {
                Input = new InputModel
                {
                    Name = role.Name
                };
                return Page();
            }
            return NotFound("Not Found");
        }

        public async Task<IActionResult> OnPostAsync(string roleid)
        {
            if (roleid == null) return NotFound("Not Found");

            role = await _roleManager.FindByIdAsync(roleid);

            if (role == null) return NotFound("Not Found");


            if (!ModelState.IsValid)
            {
                return Page();
            }

            role.Name = Input.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                StatusMessage = $"Name changed successfully: {Input.Name}";
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

