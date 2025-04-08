using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Repositories;

public class BlogRepository(AppDbContext context) : IBlogRepository
{
    private readonly AppDbContext _context = context;

    

    public async Task<List<Blog>> GetBlogsAsync()
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .ToListAsync();
    }

    

    public async Task<Blog?> GetBlogAsync(Guid id)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
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
        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
    }
    

    public async Task<List<Blog>> GetBlogsByCategoryAsync(int categoryId)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
    }
    

    public async Task<List<Blog>> GetBlogsByUserAsync(Guid userId)
    {
        return await _context.Blogs
            .Include(b => b.User)
            .Include(b => b.Category)
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }
}