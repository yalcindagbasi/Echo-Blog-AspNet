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
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
    
        if(await userService.IsEmailExist(model.Email))
        {
            ModelState.AddModelError("Email","Bu e-posta adresi zaten kayıtlı.");
            return View(model);
        }
    
        if(await userService.IsUsernameExist(model.Username))
        {
            ModelState.AddModelError("Username","Bu kullanıcı adı zaten kayıtlı.");
            return View(model);
        }
    
        var imageSourceType = Request.Form["imageSourceType"].ToString();
        if (imageSourceType == "upload" && model.ProfilePhotoFile != null)
        {
            string fileName = await SaveProfileImage(model.ProfilePhotoFile);
            model.ProfilePhotoUrl = "/images/profiles/" + fileName;
        }
        else if (imageSourceType == "url" && !string.IsNullOrEmpty(Request.Form["ProfilePhotoUrl"].ToString()))
        {
            model.ProfilePhotoUrl = Request.Form["ProfilePhotoUrl"].ToString();
        }
        else
        {
            model.ProfilePhotoUrl = "/images/profiles/default-profile.png";
        }
    
        var result = await userService.RegisterUserAsync(model);
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
            ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
        }
        return View(model);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await userService.LogoutUserAsync();
        return RedirectToAction("Index", "Home");
    }
    private async Task<string> SaveProfileImage(IFormFile imageFile)
    {
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }

        return uniqueFileName;
    }
}