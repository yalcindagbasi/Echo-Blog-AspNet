using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public interface ICategoryService
{
    Task<bool> AddCategoryAsync(Category category);
    Task<bool> UpdateCategoryAsync(Category category);
    Task<Result> DeleteCategoryAsync(int id);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<List<Category>> GetAllCategories();
}