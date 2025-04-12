using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

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
    Task<UserViewModel> GetUserViewModelAsync(AppUser user);
    Task<List<UserViewModel>> GetBestWriters(int count=4);
    Task<bool> UpdateUserAsync(EditUserViewModel model);
    Task<List<UserViewModel>> GetAllUsers();
    Task<bool> UpdateUserRolesAsync(Guid userId, string[] roles);
    Task<bool> AddToRolesAsync(AppUser user, string[] roles);
    Task<bool> RemoveFromRolesAsync(AppUser user, List<string> roles);
    Task<List<string>> GetRolesAsync(AppUser user);
    Task<bool> DeleteAsync(AppUser user);
    Task<bool> CreateRoleAsync(string roleName);
    Task<PageUserViewModel> GetPaginatedUsersAsync(int page = 1, int pageSize = 10);
    Guid? GetCurrentUserId();
    Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword);


}