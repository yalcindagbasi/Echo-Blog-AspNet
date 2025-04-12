using BlogAspNet.Web.Models.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogAspNet.Web.Models.Services.ViewModels;

public class BlogEditViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    [ValidateNever] public SelectList CategoryList { get; set; } = null!;
    
    
    public IFormFile? ImageFile { get; set; }
    public string? ImageUrlString { get; set; }
}