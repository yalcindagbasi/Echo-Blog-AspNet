using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;

namespace BlogAspNet.Web.Models.Entities;

public class Blog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required, StringLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Content { get; set; } = null!;

    [StringLength(200)] public string? ImageUrl { get; set; }  

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    

    [Required] 
    public required Guid UserId { get; set; }
    [Required] 
    public AppUser User { get; set; }

    [Required] 
    public int CategoryId { get; set; }
    
    public int ViewCount { get; set; }

    [ForeignKey("CategoryId")]
    public required Category Category { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

}