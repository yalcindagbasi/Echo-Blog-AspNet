using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlogAspNet.Web.Models.Repositories.Entities;

namespace BlogAspNet.Web.Models.Repositories;

public class Blog
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required, StringLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    public string Content { get; set; } = null!;

    [StringLength(200)]
    public string? ImageUrl { get; set; } = "https://www.svgrepo.com/show/508699/landscape-placeholder.svg";

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    public DateTime? DeletedAt { get; set; }

    [Required] 
    public required Guid UserId { get; set; }
    [Required] 
    public required AppUser User { get; set; }

    [Required] 
    public int CategoryId { get; set; }
    

    [ForeignKey("CategoryId")]
    public required Category Category { get; set; }
}