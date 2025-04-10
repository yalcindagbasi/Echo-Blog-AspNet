using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUserService _userService;

    public CommentService(ICommentRepository commentRepository, IUserService userService)
    {
        _commentRepository = commentRepository;
        _userService = userService;
    }

    public async Task<List<CommentViewModel>> GetBlogCommentsAsync(Guid blogId)
    {
        var comments = await _commentRepository.GetBlogCommentsAsync(blogId);
        return comments.Select(GetCommentViewModel).ToList();
    }

    public async Task<CommentViewModel> AddCommentAsync(CommentCreateViewModel model, Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
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

        var addedComment = await _commentRepository.AddCommentAsync(comment);

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

    public async Task<bool> DeleteCommentAsync(Guid commentId, Guid userId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null || comment.UserId != userId)
        {
            return false;
        }

        await _commentRepository.DeleteCommentAsync(comment);
        return true;
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