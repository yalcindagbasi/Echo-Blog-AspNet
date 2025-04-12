using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using BlogAspNet.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly UserManager<AppUser> _userManager;

    public UserController(IUserService userService, UserManager<AppUser> userManager)
    {
    
        _userService = userService;
        _userManager     = userManager;  
    }

   

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile(Guid? id = null)
    {
        var userId = id ?? _userService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Auth");
        }

        try
        {
            var user = await _userService.GetUserById(userId.Value);
            var userViewModel = await _userService.GetUserViewModelAsync(user);
            return View(userViewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit()
    {
        var userId = _userService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Auth");
        }

        var user = await _userService.GetUserById(userId.Value);
        var model = new EditUserViewModel
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email, // Email gelir ama değiştirilemez
            FullName = user.FullName,
            AboutMe = user.AboutMe,
            BirthDate = user.BirthDate ?? DateTime.Now,
            ProfilePhotoUrl = user.ProfilePhotoUrl
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Email değişikliğini engelleme
        var user = await _userService.GetUserById(model.Id);
        model.Email = user.Email; // Her zaman mevcut e-posta adresini kullan

        // Profil fotoğrafı işlemleri
        if (model.ProfilePhotoFile != null)
        {
            var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + 
                           Path.GetExtension(model.ProfilePhotoFile.FileName);
            var filePath = Path.Combine("wwwroot", "images", "profiles", fileName);
            
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfilePhotoFile.CopyToAsync(stream);
            }
            
            model.ProfilePhotoUrl = "/images/profiles/" + fileName;
        }

        var success = await _userService.UpdateUserAsync(model);
        if (success)
        {
            TempData["SuccessMessage"] = "Profil bilgileriniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        TempData["ErrorMessage"] = "Profil güncellenirken bir hata oluştu.";
        return View(model);
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
    [HttpGet]
    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = _userService.GetCurrentUserId();
        if (!userId.HasValue)
        {
            return RedirectToAction("Login", "Auth");
        }

        var result = await _userService.ChangePasswordAsync(userId.Value, model.CurrentPassword, model.NewPassword);
    
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Şifreniz başarıyla değiştirildi.";
            return RedirectToAction("Profile");
        }
    
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
    
        return View(model);
    }
}