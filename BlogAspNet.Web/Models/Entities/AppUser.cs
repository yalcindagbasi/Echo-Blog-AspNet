using System.ComponentModel.DataAnnotations;
using BlogAspNet.Web.Models.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BlogAspNet.Web.Models.Entities;

public class AppUser: IdentityUser<Guid>
{
    public DateTime? BirthDate { get; set; }
    [Url, MaxLength(400)] 
    public string? ProfilePhotoUrl { get; set; } 
    
    [MaxLength(4000)]
    public string? AboutMe { get; set; }

    public List<Blog> Blogs { get; set; } = new();
    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}