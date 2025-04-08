using BlogAspNet.Web.Models.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlogAspNet.Web.Models.Services.ViewModels;

public class BlogCreateViewModel
{
    public string? Title { get; set; } 
    public string? Content { get; set; }
    public string? ImageUrl { get; set; } = "https://www.svgrepo.com/show/508699/landscape-placeholder.svg";
    public int? CategoryId { get; set; }

    [ValidateNever] public SelectList CategoryList { get; set; } = null!;
}