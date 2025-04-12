using BlogAspNet.Web.Models.Entities;

namespace BlogAspNet.Web.Models.Repositories;

public interface IBlogRepository
{   
    Task<List<Blog>> GetBlogsAsync();
    Task<Blog?> GetBlogAsync(Guid id);
    Task AddBlogAsync(Blog blog);
    Task UpdateBlogAsync(Blog blog);
    Task DeleteBlogAsync(Blog blog);
    Task<List<Blog>> GetBlogsByCategoryAsync(int categoryId);
    Task<List<Blog>> GetBlogsByUserAsync(Guid userId);
    Task<(List<Blog>, int totalCount)> GetBlogsWithFilteringAsync(int? categoryId, string? searchTerm, string? sortBy, string? sortDirection, int page, int pageSize);
    Task IncrementViewCountAsync(Guid blogId);
    Task<List<Blog>> GetFeaturedBlogsAsync(int count = 3);
}