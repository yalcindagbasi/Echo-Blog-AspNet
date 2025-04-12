using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlogAspNet.Web.Models.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<CommentService> _logger;

    public CommentService(
        ICommentRepository commentRepository, 
        UserManager<AppUser> userManager,
        ILogger<CommentService> logger)
    {
        _commentRepository = commentRepository;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<List<CommentViewModel>> GetBlogCommentsAsync(Guid blogId)
    {
        var comments = await _commentRepository.GetBlogCommentsAsync(blogId);
        return comments.Select(GetCommentViewModel).ToList();
    }

    public async Task<CommentViewModel> AddCommentAsync(CommentCreateViewModel model, Guid userId)
    {
        if (model == null || string.IsNullOrWhiteSpace(model.Content))
        {
            _logger.LogWarning("Attempted to add a comment with invalid data.");
            return null;
        }

        var user = await _userManager.Users
            .Include(u => u.Comments)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            _logger.LogWarning($"User with ID {userId} not found.");
            return null;
        }

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = model.Content,
            CreatedAt = DateTime.Now,
            BlogId = model.BlogId,
            UserId = userId
        };

        try
        {
            var addedComment = await _commentRepository.AddCommentAsync(comment);
            _logger.LogInformation($"Comment with ID {addedComment.Id} successfully added by user {user.UserName}.");

            return new CommentViewModel
            {
                Id = addedComment.Id,
                Content = addedComment.Content,
                CreatedAt = addedComment.CreatedAt,
                UserId = addedComment.UserId,
                UserName = user.UserName,
                UserProfilePhotoUrl = user.ProfilePhotoUrl,
                BlogId = addedComment.BlogId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while adding comment: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        

        try
        {
            await _commentRepository.DeleteCommentAsync(comment);
            _logger.LogInformation($"Comment with ID {commentId} successfully deleted by user {userId}.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error occurred while deleting comment with ID {commentId}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsCommentOwnerAsync(Guid commentId, Guid userId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        return comment != null && comment.UserId == userId;
    }

    public async Task<List<CommentViewModel>> GetUserCommentsAsync(Guid userId)
    {
        var comments = await _commentRepository.GetUserCommentsAsync(userId);
        return comments.Select(GetCommentViewModel).ToList();
    }

    private CommentViewModel GetCommentViewModel(Comment comment)
    {
        return new CommentViewModel
        {
            Id = comment.Id,
            Content = comment.Content,
            CreatedAt = comment.CreatedAt,
            UpdatedAt = comment.UpdatedAt,
            UserId = comment.UserId,
            UserName = comment.User?.UserName,
            UserProfilePhotoUrl = comment.User?.ProfilePhotoUrl,
            BlogId = comment.BlogId
        };
    }

    public async Task<List<CommentViewModel>> GetAllCommentsAsync()
    {
        var comments = await _commentRepository.GetAllCommentsAsync();
        return comments.Select(GetCommentViewModel).ToList();
    }
}
