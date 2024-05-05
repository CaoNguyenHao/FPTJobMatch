using System;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FPTJobMatch.Areas.Admin.Models
{
    public class RolePageModel : PageModel
    {
        protected readonly RoleManager<IdentityRole> _roleManager;
        protected readonly ApplicationDbContext _dbContext;

        [TempData]
        public string StatusMessage { get; set; }

        public RolePageModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext)
        {
            _roleManager = roleManager;
            _dbContext = applicationDbContext;
        }
    }


}
