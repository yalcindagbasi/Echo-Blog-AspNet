using System.Security.Claims;
using BlogAspNet.Web.Models.Services;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAspNet.Web.Controllers;

[Authorize]
public class CommentController : Controller
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CommentCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "Yorum eklenirken bir hata oluştu. Lütfen tüm alanları kontrol edin.";
            return RedirectToAction("Index", "Blog", new { blogId = model.BlogId });
        }

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        model.UserId = Guid.Parse(userId); 
        await _commentService.AddCommentAsync(model, Guid.Parse(userId));

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true });
        }

        return RedirectToAction("Index", "Blog", new { blogId = model.BlogId });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, Guid blogId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isAdmin = User.IsInRole("Admin");
        var isOwner = await _commentService.IsCommentOwnerAsync(id, Guid.Parse(userId));
        if (!isAdmin && !isOwner)
            return Forbid();

        await _commentService.DeleteCommentAsync(id, Guid.Parse(userId));
        return RedirectToAction("Index", "Blog", new { blogId });
    }

    [HttpGet]
    public async Task<IActionResult> UserComments()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized();
        }

        var comments = await _commentService.GetUserCommentsAsync(Guid.Parse(userId));
        return View(comments);
    }
}