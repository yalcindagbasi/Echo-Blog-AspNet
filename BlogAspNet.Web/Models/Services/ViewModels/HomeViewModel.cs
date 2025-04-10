using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;

namespace BlogAspNet.Web.Models.Services.ViewModels;

public class HomeViewModel
{
    public List<BlogViewModel> Blogs { get; set; }
    public List<Category> CategoryList { get; set; }
    public List<UserViewModel> BestWriters { get; set; }
}