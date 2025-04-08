namespace BlogAspNet.Web.Models.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;
    
    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }
    public Category? GetCategory(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }
    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }
    public void DeleteCategory(Category category)
    {
        _context.Categories.Remove(category);
        _context.SaveChanges();
    }
    
    
}