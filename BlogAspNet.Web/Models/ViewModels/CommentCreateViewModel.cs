using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.Web.Models.Services.ViewModels;

public class CommentCreateViewModel
{
    public string Content { get; set; } = null!;
    public Guid UserId { get; set; }
    public Guid BlogId { get; set; }
}