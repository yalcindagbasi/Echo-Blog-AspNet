using BlogAspNet.Web.Models.Repositories;

namespace BlogAspNet.Web.Models.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> AddCategoryAsync(Category category)
    {
        return await _categoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        return await _categoryRepository.DeleteAsync(id);
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        return await _categoryRepository.GetByIdAsync(id);
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAllAsync();
    }

    

    
}