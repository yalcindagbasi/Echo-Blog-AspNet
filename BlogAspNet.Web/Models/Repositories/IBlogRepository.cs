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
}