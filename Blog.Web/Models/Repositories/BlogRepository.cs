namespace Blog.Web.Models.Repositories;

public class BlogRepository(AppDbContext context) : IBlogRepository
{
    private readonly AppDbContext _context = context;
    
    public List<Blog> GetBlogs()
    {
        return _context.Blogs.ToList();
    }
    
    public Blog? GetBlog(Guid id)
    {
        return _context.Blogs.FirstOrDefault(b => b.Id == id);
    }
    
    public void AddBlog(Blog blog)
    {
        _context.Blogs.Add(blog);
        _context.SaveChanges();
    }
    public void UpdateBlog(Blog blog)
    {
        _context.Blogs.Update(blog);
        _context.SaveChanges();
    }
    public void DeleteBlog(Blog blog)
    {
        blog.IsDeleted = true;
        blog.DeletedAt = DateTime.UtcNow;
        _context.Blogs.Update(blog);
        _context.SaveChanges();
    }
    public List<Blog> GetBlogsByCategory(int categoryId)
    {
        return _context.Blogs.Where(b => b.CategoryId == categoryId).ToList();
    }
    public List<Blog> GetBlogsByUser(Guid userId)
    {
        return _context.Blogs.Where(b => b.UserId == userId).ToList();
    }
    
    
    
}