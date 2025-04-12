using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    
    [Required, StringLength(100)]
    public string Name { get; set; } = null!;
    public List<Blog> Blogs { get; set; } = new();
}