using System.ComponentModel.DataAnnotations;
using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories.Entities;

namespace BlogAspNet.Web.Models.Repositories;

public class Comment
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(1000)] public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }

    public Blog Blog { get; set; } = null!;
    public AppUser User { get; set; } = null!;

}