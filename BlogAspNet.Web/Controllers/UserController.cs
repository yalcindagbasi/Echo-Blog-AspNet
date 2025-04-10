using BlogAspNet.Web.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Profile()
    {
        //current user's id
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.GetUserById(Guid.Parse(userId));
        if (user == null)
        {
            return NotFound();
        }
        var model = _userService.GetUserViewModel(user);
        return View(model);
    }
}