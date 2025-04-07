using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Web.Models.Repositories.Entities;

namespace Blog.Web.Models.Repositories;

public class Blog
{
    [Key]
    public Guid Id { get; set; }
    
    [Required, StringLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Content { get; set; } = null!;
    [StringLength(200)]
    public string? ImageUrl { get; set; } = "https://www.svgrepo.com/show/508699/landscape-placeholder.svg";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }
    
    [Required]
    public required AppUser User { get; set; }
    
    [Required]
    public required Guid UserId { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; } = null!;

}