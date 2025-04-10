using BlogAspNet.Web.Models.Repositories;

namespace BlogAspNet.Web.Models.Services;

public class AdminService(
    IBlogService blogService,
    ICategoryService categoryService,
    IUserService userService,
    ICommentService commentService)
    : IAdminService
{
    private readonly ICategoryService _categoryService;
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;
    private readonly ICommentService _commentService;
    
    public async Task<object> GetStatistics()
    {
        var users = await _userService.GetAllUsers();
        var blogs = await _blogService.GetAllBlogs();
        var categories = await _categoryService.GetAllCategories();
        var comments = await _commentService.GetAllCommentsAsync();

        var statistics = new
        {
            userCount = users.Count,
            blogCount = blogs.Count,
            categoryCount = categories.Count,
            commentCount = comments.Count
        };

        return statistics;
    }
}