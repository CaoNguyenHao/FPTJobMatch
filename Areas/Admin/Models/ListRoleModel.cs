using System;
using FPTJobMatch.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FPTJobMatch.Areas.Admin.Models
{
    //[Authorize(Roles = "Admin")]
    public class ListRoleModel : RolePageModel
    {
        public ListRoleModel(RoleManager<IdentityRole> roleManager, ApplicationDbContext applicationDbContext) : base(roleManager, applicationDbContext)
        {

        }

        public List<IdentityRole> roles { get; set; }

        public async Task Onget()
        {
            roles = await _roleManager.Roles.ToListAsync();
        }

        public void OnPost() => RedirectToPage();
    }
}


