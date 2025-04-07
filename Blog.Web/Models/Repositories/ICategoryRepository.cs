namespace Blog.Web.Models.Repositories;

public interface ICategoryRepository
{
    List<Category> GetCategories();
    Category? GetCategory(int id);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(Category category);
}