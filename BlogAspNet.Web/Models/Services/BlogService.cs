using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogAspNet.Web.Models.Services;

public class BlogService(IBlogRepository blogRepository, ICategoryRepository categoryRepository,IUserService userService)
    : IBlogService
{
    private readonly IBlogRepository _blogRepository = blogRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<List<BlogViewModel>> GetAllBlogs(int limit=0)
    {
        var blogList = await _blogRepository.GetBlogsAsync();
        if (limit > 0)
        {
            blogList = blogList.Take(limit).ToList();
        }
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
            return null;
        }
        return GetBlogViewModel(blog);
    }

    
    public async Task<bool> CreateBlog(BlogCreateViewModel model, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(model.Title) || string.IsNullOrWhiteSpace(model.Content))
            return false;

        var user = await userService.GetUserById(userId);
        if (user == null) return false;

        var category = await _categoryRepository.GetByIdAsync(model.CategoryId ?? 1);
        if (category == null) return false;

        var newBlog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = model.Title!,
            Content = model.Content!,
            ImageUrl = model.ImageUrl ?? "https://www.svgrepo.com/show/508699/landscape-placeholder.svg",
            CategoryId = category.Id,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Category = category,
            IsDeleted = false,
            User = user
        };

        await _blogRepository.AddBlogAsync(newBlog);
        return true;
    }

    public async Task<BlogCreateViewModel> CreateViewModel()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var createBlogViewModel = new BlogCreateViewModel();
        createBlogViewModel.CategoryList = new SelectList(categories, "Id", "Name");
        return createBlogViewModel;
    }
    public async Task<BlogViewModel> GetBlogViewModelAsync(Blog blog, ICommentService commentService)
    {
        var comments = await commentService.GetBlogCommentsAsync(blog.Id);
    
        var blogViewModel = new BlogViewModel
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
        return blogViewModel;
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
            AuthorImageUrl = blog.User.ProfilePhotoUrl,
            AuthorId = blog.User.Id,
            Category = blog.Category.Name,
            CategoryId = blog.CategoryId,
            ViewCount = blog.ViewCount
        };
        return blogViewModel;
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
    
    public async Task<List<BlogViewModel>> GetBlogsByCategory(int categoryId, int limit=0)
    {
        var blogs = await _blogRepository.GetBlogsByCategoryAsync(categoryId);
        if (limit > 0)
        {
            blogs = blogs.Take(limit).ToList();
        }
        var blogViewModels = new List<BlogViewModel>();
        foreach (var blog in blogs)
        {
            blogViewModels.Add(GetBlogViewModel(blog));
        }
        return blogViewModels;
    }

    public async Task<BlogEditViewModel> GetBlogEditViewModel(Guid blogId)
    {
        var blog = await _blogRepository.GetBlogAsync(blogId); 
        if (blog == null) return null;
    
        var categories = await _categoryRepository.GetAllAsync();
    
        var editBlogViewModel = new BlogEditViewModel
        {
            Id = blog.Id,
            Title = blog.Title,
            Content = blog.Content,
            ImageUrl = blog.ImageUrl,
            CategoryId = blog.CategoryId,
            CategoryList = new SelectList(categories, "Id", "Name", blog.CategoryId)
        };
    
        return editBlogViewModel;
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
        var users = await userService.GetBestWriters();
        return users;
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
        var homeViewModel = new HomeViewModel
        {
            Blogs = blogs.Take(blogCount).Select(b => GetBlogViewModel(b)).ToList(),
            CategoryList = categories,
            BestWriters = await GetBestWriters(),
            FeaturedBlogs = featuredBlogs.Select(b => GetBlogViewModel(b)).ToList(),
        };
        return homeViewModel;
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



