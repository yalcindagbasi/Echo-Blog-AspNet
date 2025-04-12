using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly AppDbContext _context;

    public BlogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Blog>> GetBlogsAsync()
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<Blog?> GetBlogAsync(Guid id)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }

    public async Task AddBlogAsync(Blog blog)
    {
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateBlogAsync(Blog blog)
    {
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBlogAsync(Blog blog)
    {
        blog.IsDeleted = true;
        blog.DeletedAt = DateTime.UtcNow;
        _context.Blogs.Update(blog);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Blog>> GetBlogsByCategoryAsync(int categoryId)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<List<Blog>> GetBlogsByUserAsync(Guid userId)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.UserId == userId && !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<(List<Blog>, int totalCount)> GetBlogsWithFilteringAsync(
        int? categoryId,
        string? searchTerm,
        string? sortBy,
        string? sortDirection,
        int page,
        int pageSize)
    {
        IQueryable<Blog> query = _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted);

        if (categoryId.HasValue && categoryId.Value > 0)
        {
            query = query.Where(b => b.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(b =>
                b.Title.Contains(searchTerm) ||
                b.Content.Contains(searchTerm));
        }

        int total = await query.CountAsync();

        query = sortBy?.ToLower() switch
        {
            "title" => sortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(b => b.Title)
                : query.OrderBy(b => b.Title),

            "popular" => sortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(b => b.ViewCount)
                : query.OrderBy(b => b.ViewCount),

            _ => sortDirection?.ToLower() == "desc"
                ? query.OrderByDescending(b => b.CreatedAt)
                : query.OrderBy(b => b.CreatedAt)
        };

        var blogs = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (blogs, total);
    }

    public async Task IncrementViewCountAsync(Guid blogId)
    {
        var blog = await _context.Blogs.FindAsync(blogId);
        if (blog != null && !blog.IsDeleted)
        {
            blog.ViewCount++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Blog>> GetFeaturedBlogsAsync(int count = 3)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.ViewCount)
            .Take(count)
            .ToListAsync();
    }
}
