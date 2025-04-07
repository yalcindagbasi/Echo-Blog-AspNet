using Blog.Web.Models.Repositories.Entities;
using Blog.Web.Models.Services.ViewModels;

namespace Blog.Web.Models.Services;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterViewModel model);
    Task<bool> LoginUserAsync(string email, string password);
    Task<bool> LogoutUserAsync();
    Task<bool> IsEmailExist(string email);
    Task<bool> IsUsernameExist(string username);
}