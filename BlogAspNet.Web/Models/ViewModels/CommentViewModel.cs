namespace BlogAspNet.Web.Models.Services.ViewModels;

public class CommentViewModel
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string UserProfilePhotoUrl { get; set; }
    public Guid BlogId { get; set; }
}