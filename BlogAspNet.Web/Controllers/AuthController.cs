using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class AuthController(IUserService userService) : Controller
{
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        if(userService.IsEmailExist(model.Email).Result)
        {
            ModelState.AddModelError("Email","Bu e-posta adresi zaten kayıtlı.");
            return View(model);
        }
        if(userService.IsUsernameExist(model.Username).Result)
        {
            ModelState.AddModelError("Username","Bu kullanıcı adı zaten kayıtlı.");
            return View(model);
        }
        var result =  userService.RegisterUserAsync(model).Result;
        if (result) 
        {
            return RedirectToAction("Login");
        }
        ModelState.AddModelError("","Kullanıcı oluşturulamadı.");
        return View(model);

    }
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity!.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await userService.LoginUserAsync(model.Email, model.Password);
            if (result)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Geçersiz giriş.");
        }
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await userService.LogoutUserAsync();
        return RedirectToAction("Index", "Home");
    }
}