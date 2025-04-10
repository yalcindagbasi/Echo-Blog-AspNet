using BlogAspNet.Web.Models.Services.ViewModels;
            
            namespace BlogAspNet.Web.Models.Services.ViewModels
            {
                public class PageUserViewModel
                {
                    public List<UserViewModel> Users { get; set; }
                    public int PageSize { get; set; }
                    public int CurrentPage { get; set; }
                    public int TotalItems { get; set; }
                    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
                }
            }