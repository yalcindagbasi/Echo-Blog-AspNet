using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogAspNet.Web.Models.Repositories.Entities;

public class AppUser: IdentityUser<Guid>
{
    public DateTime? BirthDate { get; set; }
    [Url, MaxLength(400)] 
    public string ProfilePhotoUrl { get; set; } = null!;
    
    [MaxLength(4000)]
    public string? AboutMe { get; set; }

    public List<Blog> Blogs { get; set; } = null!;
    [Required, MaxLength(100)]
    public string FullName { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}