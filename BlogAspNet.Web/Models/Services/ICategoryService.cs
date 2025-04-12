using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;

namespace BlogAspNet.Web.Models.Services;

public interface ICategoryService
{
    Task<bool> AddCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int id);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<List<Category>> GetAllCategories();
}