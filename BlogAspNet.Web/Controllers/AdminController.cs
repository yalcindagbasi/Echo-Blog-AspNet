using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Controllers;

public class AdminController(
    ILogger<AdminController> logger,
    ICategoryService categoryService,
    IUserService userService,
    IAdminService adminService,
    IBlogService blogService)
    : Controller
{
    private readonly ILogger<AdminController> _logger = logger;

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 5)
    {
        var pageUserViewModel = await userService.GetPaginatedUsersAsync(page, pageSize);
        return View(pageUserViewModel);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetStatistics()
    {
        var statistics = await adminService.GetStatistics();
        return Json(statistics);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrEmpty(roleName))
        {
            TempData["ErrorMessage"] = "Rol adı boş olamaz.";
            return RedirectToAction("Users");
        }

        await userService.CreateRoleAsync(roleName);

        TempData["SuccessMessage"] = $"{roleName} rolü başarıyla oluşturuldu.";
        return RedirectToAction("Users");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Users(int page = 1, int pageSize = 10)
    {
        var users = await userService.GetPaginatedUsersAsync(page, pageSize);
        return View(users);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetUserRoles(Guid userId)
    {
        var user = await userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound();
        }
        
        var roles = await userService.GetRolesAsync(user);
        return Json(roles);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateUserRole(Guid userId, string[] roles)
    {
        var user = await userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound();
        }
        
        var userRoles = await userService.GetRolesAsync(user);
        await userService.RemoveFromRolesAsync(user, userRoles);
        
        await userService.AddToRolesAsync(user, roles);
        
        TempData["SuccessMessage"] = "Kullanıcı rolleri başarıyla güncellendi.";
        return RedirectToAction("Users");
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        var user = await userService.GetUserById(userId);
        if (user == null)
        {
            return NotFound();
        }
        
        await userService.DeleteAsync(user);
        
        TempData["SuccessMessage"] = "Kullanıcı başarıyla silindi.";
        return RedirectToAction("Users");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Categories()
    {
        var categories = await categoryService.GetAllCategories();
        return View(categories);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateCategory(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            TempData["ErrorMessage"] = "Kategori adı boş olamaz.";
            return RedirectToAction("Categories");
        }

        var category = new Category { Name = name };
        await categoryService.AddCategoryAsync(category);

        TempData["SuccessMessage"] = $"{name} kategorisi başarıyla oluşturuldu.";
        return RedirectToAction("Categories");
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await categoryService.GetCategoryByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        await categoryService.DeleteCategoryAsync(id);

        TempData["SuccessMessage"] = $"{category.Name} kategorisi başarıyla silindi.";
        return RedirectToAction("Categories");
    }
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Blogs(int page = 1, int pageSize = 10)
    {
        var blogsResult = await blogService.GetPaginatedBlogsAsync(page, pageSize);
    
        ViewData["CurrentPage"] = page;
        ViewData["PageSize"] = pageSize;
        ViewData["TotalItems"] = blogsResult.totalCount;
        ViewData["TotalPages"] = Math.Ceiling((double)blogsResult.totalCount / pageSize);
    
        return View(blogsResult.blogs);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteBlog(Guid blogId)
    {
        var blog = await blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }

        var success = await blogService.DeleteBlog(blogId);
        if (!success)
        {
            TempData["ErrorMessage"] = "Blog silinirken bir hata oluştu.";
        }
        else
        {
            TempData["SuccessMessage"] = "Blog başarıyla silindi.";
        }

        return RedirectToAction("Blogs");
    }
    
}
