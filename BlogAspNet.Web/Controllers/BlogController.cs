using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService blogService;
    private readonly ICommentService commentService;

    public BlogController(IBlogService blogService, ICommentService commentService)
    {
        this.commentService = commentService;
        this.blogService = blogService;
    }

    public async Task<IActionResult> Index(Guid blogId)
    {
        var blog = await blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }

        var comments = await commentService.GetBlogCommentsAsync(blogId);
        ViewBag.BlogId = blogId;
        ViewBag.Comments = comments;

        

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
            return View(blogService.CreateViewModel());

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        if (model.ImageFile != null)
        {
            string fileName = await SaveImage(model.ImageFile);
            model.ImageUrl = "/images/blogs/" + fileName;
        }
        else if (!string.IsNullOrEmpty(model.ImageUrlString))
        {
            model.ImageUrl = model.ImageUrlString;
        }

        var success = await blogService.CreateBlog(model, Guid.Parse(userId));
        if (!success)
        {
            ModelState.AddModelError("", "Blog oluşturulurken hata oluştu.");
            return View(blogService.CreateViewModel());
        }

        return RedirectToAction("Index", "Home");
    }

    private async Task<string> SaveImage(IFormFile imageFile)
    {
        string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "blogs");

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
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Edit(Guid blogId)
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
        
        return View(await blogService.GetBlogEditViewModel(blogId));
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(BlogEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var imageSourceType = Request.Form["imageSourceType"].ToString();
        if (imageSourceType == "upload" && model.ImageFile != null)
        {
            string fileName = await SaveImage(model.ImageFile);
            model.ImageUrl = "/images/blogs/" + fileName;
        }
        else if (imageSourceType == "url" && !string.IsNullOrEmpty(model.ImageUrlString))
        {
            model.ImageUrl = model.ImageUrlString;
        }

        var success = await blogService.UpdateBlog(model);
        if (!success)
        {
            ModelState.AddModelError("", "Blog güncellenirken hata oluştu.");
            return View(model);
        }

        return RedirectToAction("UserBlogs");
    }
    [HttpGet]
    public async Task<IActionResult> GetBlogsByCategory(int categoryId)
    {
        // Eğer categoryId 0 ise (Hepsi kategorisi), tüm blogları getir
        var blogs = categoryId == 0 
            ? await blogService.GetAllBlogs() 
            : await blogService.GetBlogsByCategory(categoryId);
    
        return PartialView("BlogList", blogs);
    }
}