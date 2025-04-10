using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.Web.Models.Repositories;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;
    public List<Blog> Blogs { get; set; } = new();
}