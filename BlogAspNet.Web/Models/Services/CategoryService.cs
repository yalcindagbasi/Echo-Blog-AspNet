using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;
using BlogAspNet.Web.Models.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBlogService _blogService;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IBlogService blogService, ILogger<CategoryService> logger)
    {
        _categoryRepository = categoryRepository;
        _blogService = blogService;
        _logger = logger;
    }

    public async Task<bool> AddCategoryAsync(Category category)
    {
        return await _categoryRepository.AddAsync(category);
    }

    public async Task<bool> UpdateCategoryAsync(Category category)
    {
        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task<Result> DeleteCategoryAsync(int id)
    {
        if (id == 1)
        {
            return new Result { Success = false, Message = "Default category cannot be deleted." };
        }

        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
        {
            return new Result { Success = false, Message = $"Category with ID {id} not found." };
        }

        try
        {
            var blogs = await _blogService.GetBlogsByCategory(id);
            foreach (var blog in blogs)
            {
                var blogEditViewModel = await _blogService.GetBlogEditViewModel(blog.Id);
                blogEditViewModel.CategoryId = 1; 
                await _blogService.UpdateBlog(blogEditViewModel);
            }

            await _categoryRepository.DeleteAsync(id);
            return new Result { Success = true, Message = "Category successfully deleted." };
        }
        catch (Exception ex)
        {
            return new Result { Success = false, Message = $"Error: {ex.Message}" };
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