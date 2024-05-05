using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class RoleController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

}

