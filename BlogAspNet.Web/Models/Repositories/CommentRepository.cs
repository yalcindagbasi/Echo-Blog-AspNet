using BlogAspNet.Web.Models.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly AppDbContext _context;

    public CommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetBlogCommentsAsync(Guid blogId)
    {
        return await _context.Comments
            .Where(c => c.BlogId == blogId)
            .Include(c => c.User)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    public async Task<Comment> AddCommentAsync(Comment comment)
    {
        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment> GetCommentByIdAsync(Guid commentId)
    {
        return await _context.Comments.FindAsync(commentId);
    }

    public async Task DeleteCommentAsync(Comment comment)
    {
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Comment>> GetUserCommentsAsync(Guid userId)
    {
        return await _context.Comments
            .Where(c => c.UserId == userId)
            .Include(c => c.Blog)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}