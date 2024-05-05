using FPTJobMatch.Areas.Admin.Models;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FPTJobMatch.Areas.Admin.Models
{
    public class CreateModel : RolePageModel
    {
        public CreateModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {
        }

        [TempData] // Sử dụng Session
        public string StatusMessage { get; set; }

        public class InputModel
        {

            [Display(Name = "Name Role")]
            [Required(ErrorMessage = "Must Input{0}")]
            [StringLength(256, MinimumLength = 3, ErrorMessage = "{0} is {2} to {1} characters long")]
            public string Name { set; get; }

        }

        [BindProperty]
        public InputModel Input { set; get; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
            }
            var newRole = new IdentityRole(Input.Name);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"New role created successfully: {Input.Name}";
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
