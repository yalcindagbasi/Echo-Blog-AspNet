using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userService.GetUserById(Guid.Parse(userId));
        if (user == null)
        {
            return NotFound();
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Username = user.UserName,
            FullName = user.FullName,
            Email = user.Email,
            BirthDate = user.BirthDate ?? DateTime.Now,
            AboutMe = user.AboutMe,
            ProfilePhotoUrl = user.ProfilePhotoUrl
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(EditUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || model.Id != Guid.Parse(userId))
        {
            return Unauthorized();
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

        var result = await _userService.UpdateUserAsync(model);
        if (result)
        {
            TempData["SuccessMessage"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction("Profile");
        }

        ModelState.AddModelError("", "Profil güncellenirken bir hata oluştu.");
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
}