namespace Blog.Web.Models.Repositories;

public interface IBlogRepository
{
    List<Blog> GetBlogs();
    Blog? GetBlog(Guid id);
    void AddBlog(Blog blog);
    void UpdateBlog(Blog blog);
    void DeleteBlog(Blog blog);
    List<Blog> GetBlogsByCategory(int categoryId);
    List<Blog> GetBlogsByUser(Guid userId);
}