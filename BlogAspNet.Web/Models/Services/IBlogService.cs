using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public interface IBlogService
{
    Task<List<BlogViewModel>> GetAllBlogs();
    Task<BlogViewModel> GetBlogById(Guid id);
    Task<bool> CreateBlog(BlogCreateViewModel model, Guid userId);
    Task<bool> DeleteBlog(Guid blogId);
    Task<List<BlogViewModel>> GetBlogsByUser(Guid userId);
    BlogCreateViewModel CreateViewModel();
    Task<BlogEditViewModel> GetBlogEditViewModel(Guid blogId);
    Task<bool> UpdateBlog(BlogEditViewModel model);
    Task<List<BlogViewModel>> GetBlogsByCategory(int categoryId);
    Task<BlogFilterViewModel> GetFilteredBlogsAsync(int? categoryId, string? searchTerm, string? sortBy, string? sortDirection, int page, int pageSize);
    Task<HomeViewModel> GetHomeViewModel(int blogCount = 6, int featuredBlogCount = 3);
    Task IncrementViewCountAsync(Guid blogId);




}