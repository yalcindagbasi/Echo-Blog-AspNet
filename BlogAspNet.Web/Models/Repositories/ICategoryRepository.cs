using BlogAspNet.Web.Models.Entities;

namespace BlogAspNet.Web.Models.Repositories;

public interface ICategoryRepository
{
    Task<bool> AddAsync(Category category);
    Task<bool> UpdateAsync(Category category);
    Task<bool> DeleteAsync(int id);
    Task<Category> GetByIdAsync(int id);
    Task<List<Category>> GetAllAsync();
}