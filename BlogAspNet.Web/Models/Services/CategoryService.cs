using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBlogService _blogService;

    public CategoryService(ICategoryRepository categoryRepository, IBlogService blogService)
    {
        _categoryRepository = categoryRepository;
        _blogService = blogService;
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
        if (id == 1)
        {
            return false;
        }
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }

        try
        {
            var blogs= await _blogService.GetBlogsByCategory(id);
            foreach (var blog in blogs)
            {
                
                var blogEditViewModel = await _blogService.GetBlogEditViewModel(blog.Id);
                blogEditViewModel.CategoryId = 1; // Set to default category
                await _blogService.UpdateBlog(blogEditViewModel);
            }
            await _categoryRepository.DeleteAsync(id);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
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