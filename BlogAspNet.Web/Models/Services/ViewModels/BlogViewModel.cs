namespace BlogAspNet.Web.Models.Services.ViewModels;

public class BlogViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Author { get; set; } = null!;
    public string AuthorImageUrl { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public string Category { get; set; } = null!;
    public int CategoryId { get; set; } 
}