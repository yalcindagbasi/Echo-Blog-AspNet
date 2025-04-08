using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogAspNet.Web.Models.Services;

public class BlogService(IBlogRepository blogRepository, ICategoryRepository categoryRepository,IUserService userService)
    : IBlogService
{
    private readonly IBlogRepository _blogRepository = blogRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<List<BlogViewModel>> GetAllBlogs()
    {
        var blogList = await _blogRepository.GetBlogsAsync();
        var blogViewModels = new List<BlogViewModel>();
        foreach (var blog in blogList)
        {
            blogViewModels.Add(GetBlogViewModel(blog));
        }
        return blogViewModels;
    }
    public async Task<BlogViewModel> GetBlogById(Guid id)
    {
        System.Diagnostics.Debug.WriteLine($"Aranan blog ID: {id}");
    
        var blog = await _blogRepository.GetBlogAsync(id);
        if (blog == null)
        {
            throw new Exception($"Blog not found. ID: {id}");
        }
        return GetBlogViewModel(blog);
    }

    
    public async Task<bool> CreateBlog(BlogCreateViewModel model, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Content))
            return false;

        var user = await userService.GetUserById(userId);
        if (user == null) return false;

        var category = _categoryRepository.GetCategory(model.CategoryId ?? 1);
        if (category == null) return false;

        var newBlog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = model.Title!,
            Content = model.Content!,
            ImageUrl = model.ImageUrl,
            CategoryId = category.Id,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Category = category,
            User = user
        };

        await _blogRepository.AddBlogAsync(newBlog);
        return true;
    }

    public BlogCreateViewModel CreateViewModel()
    {
        var categories = _categoryRepository.GetCategories();
        var createBlogViewModel = new BlogCreateViewModel();
        createBlogViewModel.CategoryList = new SelectList(categories, "Id", "Name");
        return createBlogViewModel;
    }
    public BlogViewModel GetBlogViewModel(Blog blog)
    {
        var blogViewModel = new BlogViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            Author = blog.User.UserName,
            AuthorId = blog.User.Id,
            Category = blog.Category.Name
        };
        return blogViewModel;
    }
    public async Task<HomeViewModel> GetHomeViewModel()
    {
        var blogs = await _blogRepository.GetBlogsAsync();
        var homeViewModel = new HomeViewModel
        {
            Blogs = blogs.Select(b => GetBlogViewModel(b)).ToList()
        };
        return homeViewModel;
    }
    public async Task<List<BlogViewModel>> GetBlogsByUser(Guid userId)
    {
        var blogs =  await _blogRepository.GetBlogsByUserAsync(userId);
        Console.WriteLine("User ID: " + userId);
        var blogViewModels = new List<BlogViewModel>();
        foreach (var blog in blogs)
        {
            blogViewModels.Add(GetBlogViewModel(blog));
        }
        return blogViewModels;
    }
    public async Task<bool> DeleteBlog(Guid blogId)
    {
        var blog = await _blogRepository.GetBlogAsync(blogId);
        if (blog == null) return false;

        await _blogRepository.DeleteBlogAsync(blog);
        return true;
    }

}



