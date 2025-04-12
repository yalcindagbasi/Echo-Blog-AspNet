using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public interface IBlogService
{
    Task<List<BlogViewModel>> GetAllBlogs(int limit = 0);
    Task<BlogViewModel> GetBlogById(Guid id);
    Task<bool> CreateBlog(BlogCreateViewModel model, Guid userId);
    Task<bool> DeleteBlog(Guid blogId);
    Task<List<BlogViewModel>> GetBlogsByUser(Guid userId);
    Task<BlogCreateViewModel> CreateViewModel();
    Task<BlogEditViewModel> GetBlogEditViewModel(Guid blogId);
    Task<bool> UpdateBlog(BlogEditViewModel model);
    Task<List<BlogViewModel>> GetBlogsByCategory(int categoryId, int limit=0);
    Task<BlogFilterViewModel> GetFilteredBlogsAsync(BlogFilterViewModel filter);
    Task<HomeViewModel> GetHomeViewModel(int blogCount = 6, int featuredBlogCount = 3);
    Task IncrementViewCountAsync(Guid blogId);
    Task<BlogViewModel> GetBlogViewModelAsync(Blog blog, ICommentService commentService);
    Task<(List<BlogViewModel> blogs, int totalCount)> GetPaginatedBlogsAsync(int page, int pageSize);
    Task<List<BlogViewModel>> GetRecentBlogsAsync(int count);





}