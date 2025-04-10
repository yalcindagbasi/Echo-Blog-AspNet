using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class AdminController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }
}