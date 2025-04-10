using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public interface ICommentService
{
    Task<List<CommentViewModel>> GetBlogCommentsAsync(Guid blogId);
    Task<CommentViewModel> AddCommentAsync(CommentCreateViewModel model, Guid userId);
    Task<bool> DeleteCommentAsync(Guid commentId, Guid userId);
    Task<bool> IsCommentOwnerAsync(Guid commentId, Guid userId);
    Task<List<CommentViewModel>> GetUserCommentsAsync(Guid userId);
    Task<List<CommentViewModel>> GetAllCommentsAsync();
}