using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogAspNet.Web.Models.Services;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUserService _userService;

    public BlogService(IBlogRepository blogRepository, ICategoryRepository categoryRepository, IUserService userService)
    {
        _blogRepository = blogRepository;
        _categoryRepository = categoryRepository;
        _userService = userService;
    }

    public async Task<List<BlogViewModel>> GetAllBlogs(int limit = 0)
    {
        var blogList = await _blogRepository.GetBlogsAsync();
        if (limit > 0)
        {
            blogList = blogList.Take(limit).ToList();
        }

        return blogList.Select(blog => GetBlogViewModel(blog)).ToList();
    }

    public async Task<BlogViewModel> GetBlogById(Guid id)
    {
        var blog = await _blogRepository.GetBlogAsync(id);
        return blog == null ? null : GetBlogViewModel(blog);
    }

    public async Task<bool> CreateBlog(BlogCreateViewModel model, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Content)) return false;

        var user = await _userService.GetUserById(userId);
        if (user == null) return false;

        var category = await _categoryRepository.GetByIdAsync(model.CategoryId ?? 1);
        if (category == null) return false; 

        var newBlog = new Blog
        {
            Title = model.Title!,
            Content = model.Content!,
            ImageUrl = model.ImageUrl ?? "https://www.svgrepo.com/show/508699/landscape-placeholder.svg",
            CategoryId = category.Id,
            Category = category, 
            UserId = userId,
            User = user, 
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        await _blogRepository.AddBlogAsync(newBlog);
        return true;
    }

    public async Task<BlogCreateViewModel> CreateViewModel()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var createBlogViewModel = new BlogCreateViewModel
        {
            CategoryList = new SelectList(categories, "Id", "Name")
        };
        return createBlogViewModel;
    }

    public async Task<BlogViewModel> GetBlogViewModelAsync(Blog blog, ICommentService commentService)
    {
        var comments = await commentService.GetBlogCommentsAsync(blog.Id);

        return new BlogViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            Author = blog.User.UserName,
            AuthorImageUrl = blog.User.ProfilePhotoUrl,
            AuthorId = blog.User.Id,
            Category = blog.Category.Name,
            CategoryId = blog.CategoryId,
            ViewCount = blog.ViewCount,
            Comments = comments
        };
    }

    public BlogViewModel GetBlogViewModel(Blog blog)
    {
        return new BlogViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            CreatedAt = blog.CreatedAt,
            UpdatedAt = blog.UpdatedAt,
            Author = blog.User.UserName,
            AuthorImageUrl = blog.User.ProfilePhotoUrl,
            AuthorId = blog.User.Id,
            Category = blog.Category.Name,
            CategoryId = blog.CategoryId,
            ViewCount = blog.ViewCount
        };
    }

    public async Task<List<BlogViewModel>> GetBlogsByUser(Guid userId)
    {
        var blogs = await _blogRepository.GetBlogsByUserAsync(userId);
        return blogs.Select(blog => GetBlogViewModel(blog)).ToList();
    }

    public async Task<bool> DeleteBlog(Guid blogId)
    {
        var blog = await _blogRepository.GetBlogAsync(blogId);
        if (blog == null) return false;

        await _blogRepository.DeleteBlogAsync(blog);
        return true;
    }

    public async Task<List<BlogViewModel>> GetBlogsByCategory(int categoryId, int limit = 0)
    {
        var blogs = await _blogRepository.GetBlogsByCategoryAsync(categoryId);
        if (limit > 0)
        {
            blogs = blogs.Take(limit).ToList();
        }

        return blogs.Select(blog => GetBlogViewModel(blog)).ToList();
    }

    public async Task<BlogEditViewModel> GetBlogEditViewModel(Guid blogId)
    {
        var blog = await _blogRepository.GetBlogAsync(blogId);
        if (blog == null) return null;

        var categories = await _categoryRepository.GetAllAsync();
        return new BlogEditViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            CategoryId = blog.CategoryId,
            CategoryList = new SelectList(categories, "Id", "Name", blog.CategoryId)
        };
    }

    public async Task<bool> UpdateBlog(BlogEditViewModel model)
    {
        var blog = await _blogRepository.GetBlogAsync(model.Id);
        if (blog == null) return false;

        blog.Title = model.Title;
        blog.Content = model.Content;
        blog.ImageUrl = model.ImageUrl ?? blog.ImageUrl;
        blog.CategoryId = model.CategoryId;
        blog.UpdatedAt = DateTime.UtcNow;

        await _blogRepository.UpdateBlogAsync(blog);
        return true;
    }

    public async Task<List<UserViewModel>> GetBestWriters()
    {
        return await _userService.GetBestWriters();
    }

    public async Task<BlogFilterViewModel> GetFilteredBlogsAsync(BlogFilterViewModel filter)
    {
        var categories = await _categoryRepository.GetAllAsync();
        var sortOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "date", Text = "Tarihe Göre" },
            new SelectListItem { Value = "title", Text = "Başlığa Göre" },
            new SelectListItem { Value = "popular", Text = "Popülerliğe Göre" }
        };

        var result = await _blogRepository.GetBlogsWithFilteringAsync(
            filter.CategoryId, filter.SearchTerm, filter.SortBy,
            filter.SortDirection, filter.Page, filter.PageSize);

        filter.CategoryList = new SelectList(categories, "Id", "Name", filter.CategoryId);
        filter.SortOptions = new SelectList(sortOptions, "Value", "Text", filter.SortBy);
        filter.Blogs = result.Item1.Select(b => GetBlogViewModel(b)).ToList();
        filter.TotalBlogs = result.Item2;

        return filter;
    }

    public async Task<HomeViewModel> GetHomeViewModel(int blogCount = 6, int featuredBlogCount = 3)
    {
        var blogs = await _blogRepository.GetBlogsAsync();
        var featuredBlogs = await _blogRepository.GetFeaturedBlogsAsync(featuredBlogCount);
        var categories = await _categoryRepository.GetAllAsync();

        return new HomeViewModel
        {
            Blogs = blogs.Take(blogCount).Select(b => GetBlogViewModel(b)).ToList(),
            CategoryList = categories,
            BestWriters = await GetBestWriters(),
            FeaturedBlogs = featuredBlogs.Select(b => GetBlogViewModel(b)).ToList(),
        };
    }

    public async Task IncrementViewCountAsync(Guid blogId)
    {
        await _blogRepository.IncrementViewCountAsync(blogId);
    }

    public async Task<(List<BlogViewModel> blogs, int totalCount)> GetPaginatedBlogsAsync(int page, int pageSize)
    {
        var result = await _blogRepository.GetBlogsWithFilteringAsync(null, null, "date", "desc", page, pageSize);
        var blogs = result.Item1.Select(b => GetBlogViewModel(b)).ToList();
        return (blogs, result.Item2);
    }

    public async Task<List<BlogViewModel>> GetRecentBlogsAsync(int count)
    {
        var blogs = await _blogRepository.GetBlogsAsync();
        var recentBlogs = blogs.OrderByDescending(b => b.CreatedAt).Take(count).ToList();
        return recentBlogs.Select(b => GetBlogViewModel(b)).ToList();
    }
}
