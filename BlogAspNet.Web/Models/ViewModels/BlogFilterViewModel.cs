using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.Web.Models.Services.ViewModels
{
    public class BlogFilterViewModel
    {
        public int? CategoryId { get; set; }
        
        [Display(Name = "Arama")]
        public string? SearchTerm { get; set; }
        
        [Display(Name = "Sıralama")]
        public string? SortBy { get; set; } 
        
        [Display(Name = "Sıralama Yönü")]
        public string? SortDirection { get; set; } 
        
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        
        public SelectList? CategoryList { get; set; }
        public SelectList? SortOptions { get; set; }
        public List<BlogViewModel>? Blogs { get; set; }
        public int TotalBlogs { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalBlogs / (double)PageSize);
    }
}