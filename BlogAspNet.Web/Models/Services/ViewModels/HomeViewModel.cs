using BlogAspNet.Web.Models.Repositories;
namespace BlogAspNet.Web.Models.Services.ViewModels;

public class HomeViewModel
{
    public List<BlogViewModel> Blogs { get; set; }
}