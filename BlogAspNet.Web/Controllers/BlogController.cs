using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

public class BlogController : Controller
{
    private readonly IBlogService _blogService;
    private readonly ICommentService _commentService;

    public BlogController(IBlogService blogService, ICommentService commentService)
    {
        this._commentService = commentService;
        this._blogService = blogService;
    }

    public async Task<IActionResult> Index(Guid blogId)
    {
        var blog = await _blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }

        await _blogService.IncrementViewCountAsync(blogId);

        var comments = await _commentService.GetBlogCommentsAsync(blogId);
        Console.WriteLine("Comment count: " + comments.Count);
        blog.Comments = comments;
        ViewBag.BlogId = blogId;

        ViewBag.RelatedBlogs = await _blogService.GetBlogsByCategory(blog.CategoryId,10);

        return View(blog);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create()
    {
        return View(await _blogService.CreateViewModel());
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(BlogCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(await _blogService.CreateViewModel());

        var userId = GetCurrentUserId();
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

        var success = await _blogService.CreateBlog(model, userId.Value);
        if (!success)
        {
            ModelState.AddModelError("", "Blog oluşturulurken hata oluştu.");
            return View(await _blogService.CreateViewModel());
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
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return Unauthorized();
        }

        var blogs = await _blogService.GetBlogsByUser(userId.Value);
    
        foreach (var blog in blogs)
        {
            blog.Comments = await _commentService.GetBlogCommentsAsync(blog.Id);
        }
    
        return View(blogs);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Guid blogId)
    {
        
        var blog = await _blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }

        
        var userId = GetCurrentUserId();
        bool isAdmin = User.IsInRole("Admin");
        bool isOwner = blog.AuthorId == userId;
    
        if (!isAdmin && !isOwner)
            return Forbid();

        
        var success = await _blogService.DeleteBlog(blogId);
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
        var blog = await _blogService.GetBlogById(blogId);
        if (blog == null)
        {
            return NotFound();
        }
        var userId = GetCurrentUserId();
        bool isAdmin = User.IsInRole("Admin");
        bool isOwner = blog.AuthorId == userId;
    
        if (!isAdmin && !isOwner)
            return Forbid();
        
        return View(await _blogService.GetBlogEditViewModel(blogId));
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Edit(BlogEditViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var blog = await _blogService.GetBlogById(model.Id);
        if (blog == null)
        {
            return NotFound();
        }
        var userId = GetCurrentUserId();
        bool isAdmin = User.IsInRole("Admin");
        bool isOwner = blog.AuthorId == userId;
    
        if (!isAdmin && !isOwner)
            return Forbid();

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

        var success = await _blogService.UpdateBlog(model);
        if (!success)
        {
            ModelState.AddModelError("", "Blog güncellenirken hata oluştu.");
            return View(model);
        }

        return RedirectToAction("Index", new { blogId = model.Id });
    }
    [HttpGet]
    public async Task<IActionResult> GetBlogsByCategory(int categoryId, int limit = 6)
    {
        var blogs = categoryId == 0 
            ? await _blogService.GetAllBlogs(limit) 
            : await _blogService.GetBlogsByCategory(categoryId, limit);
    
        return PartialView("_BlogList", blogs);
    }
    [HttpGet]
    public async Task<IActionResult> Explore([FromQuery] BlogFilterViewModel filter)
    {
        if (filter.Page <= 0) filter.Page = 1;
        if (filter.PageSize <= 0) filter.PageSize = 9;
    
        var model = await _blogService.GetFilteredBlogsAsync(
            filter);
    
        return View(model);
    }
    private Guid? GetCurrentUserId()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return null;
        }
        return Guid.Parse(userId);
    }
}