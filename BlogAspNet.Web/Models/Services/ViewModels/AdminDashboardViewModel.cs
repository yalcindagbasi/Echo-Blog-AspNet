namespace BlogAspNet.Web.Models.Services.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalBlogs { get; set; }
    public int TotalUsers { get; set; }
    public int TotalCategories { get; set; }
    public List<BlogViewModel> RecentBlogs { get; set; }
}

public class PaginatedViewModel<T>
{
    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}