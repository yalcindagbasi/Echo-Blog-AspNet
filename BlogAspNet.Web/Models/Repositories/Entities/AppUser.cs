using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BlogAspNet.Web.Models.Repositories.Entities;

public class AppUser: IdentityUser<Guid>
{
    public DateTime? BirthDate { get; set; }
    [Required, Url, MaxLength(400)]
    public string ProfilePhotoUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png?20150327203541";
    public List<Blog> Blogs { get; set; } = new();    
}