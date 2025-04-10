using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;

namespace BlogAspNet.Web.Models.Services;

public interface IUserService
{
    Task<bool> RegisterUserAsync(RegisterViewModel model);
    Task<bool> LoginUserAsync(string email, string password);
    Task<bool> LogoutUserAsync();
    Task<bool> IsEmailExist(string email);
    Task<bool> IsUsernameExist(string username);
    Task<AppUser> GetUserById(Guid id);
    UserViewModel GetUserViewModel(AppUser user);
    Task<List<UserViewModel>> GetBestWriters();
    
}