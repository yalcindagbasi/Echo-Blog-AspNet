using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog.Web.Models.Repositories.Entities;

public class AppUser: IdentityUser<Guid>
{
    public DateTime? BirthDate { get; set; }
    [Required, Url]
    public string ProfilePhotoUrl { get; set; } = "https://media.istockphoto.com/id/1316947194/vector/messenger-profile-icon-on-white-isolated-background-vector-illustration.jpg?s=612x612&w=0&k=20&c=1iQ926GXQTJkopoZAdYXgU17NCDJIRUzx6bhzgLm9ps=";
    public List<Blog> Blogs { get; set; } = new();    
}