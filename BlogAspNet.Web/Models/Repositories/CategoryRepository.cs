using BlogAspNet.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync()
    {
        return await _context.Categories
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
        var exists = await _context.Categories.AnyAsync(c => c.Id == category.Id);
        if (!exists) return false;

        _context.Categories.Update(category);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category is null) return false;

        _context.Categories.Remove(category);
        return await _context.SaveChangesAsync() > 0;
    }
}