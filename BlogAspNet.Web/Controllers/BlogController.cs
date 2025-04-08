using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService blogService;

    public BlogController(IBlogService blogService)
    {
        this.blogService = blogService;
    }

    public async Task<IActionResult> Index(Guid blogId)
    {
        var blog = await blogService.GetBlogById(blogId);
        return View(blog);
    }

    [HttpGet]
    [Authorize]
    public IActionResult CreateBlog()
    {
        return View(blogService.CreateViewModel());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBlog(BlogCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var success = await blogService.CreateBlog(model, Guid.Parse(userId));
        if (!success)
        {
            ModelState.AddModelError("", "Blog oluşturulurken hata oluştu.");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
    
    [Authorize]
    public async Task<IActionResult> UserBlogs()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }
    
        var blogs = await blogService.GetBlogsByUser(Guid.Parse(userId));
        return View(blogs);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Guid blogId)
    {
        
        var blog = await blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }

        
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || blog.AuthorId != Guid.Parse(userId))
        {
            return Forbid(); 
        }

        
        var success = await blogService.DeleteBlog(blogId);
        if (!success)
        {
            TempData["Error"] = "Blog silinirken bir hata oluştu.";
        }
        else
        {
            TempData["Success"] = "Blog başarıyla silindi.";
        }

        
        return RedirectToAction("UserBlogs");
    }
}