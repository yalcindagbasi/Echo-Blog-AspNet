using System.ComponentModel.DataAnnotations;
using BlogAspNet.Web.Models.Repositories.Entities;

namespace BlogAspNet.Web.Models.Repositories;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid BlogId { get; set; }
    public Guid UserId { get; set; }

    public Blog Blog { get; set; }
    public AppUser User { get; set; }
}