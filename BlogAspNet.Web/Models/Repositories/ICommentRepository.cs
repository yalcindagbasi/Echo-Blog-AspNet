namespace BlogAspNet.Web.Models.Repositories;

public interface ICommentRepository 
{
    Task<List<Comment>> GetBlogCommentsAsync(Guid blogId);
    Task<Comment> AddCommentAsync(Comment comment);
    Task<Comment> GetCommentByIdAsync(Guid commentId);
    Task DeleteCommentAsync(Comment comment);
    Task<List<Comment>> GetUserCommentsAsync(Guid userId);
}