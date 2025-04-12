using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Services;

public class UserService(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager,
    ICommentService commentService)
    : IUserService
{
    private readonly RoleManager<AppRole> _roleManager = roleManager;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    private readonly ICommentService _commentService = commentService;
    
    public async Task<bool> RegisterUserAsync(RegisterViewModel model)
    {
        
        var user = new AppUser
        {
            UserName = model.Username,
            Email = model.Email,
            BirthDate = model.BirthDate,
            FullName = model.FullName,
            CreatedDate = DateTime.Now,
            ProfilePhotoUrl = model.ProfilePhotoUrl ?? "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png?20150327203541",
            AboutMe = model.AboutMe ?? "About me",
        };
        
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync(new AppRole { Name = "User" });
            }
            await _userManager.AddToRoleAsync(user, "User");
            return true;
        }
        
        return false;
    }
    
    public async Task<bool> LoginUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
        return result.Succeeded;
    }
    public async Task<bool> LogoutUserAsync()
    {
        await _signInManager.SignOutAsync();
        return true;
    }
    public async Task<bool> IsEmailExist(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null;
    }
    public async Task<bool> IsUsernameExist(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        return user != null;
    }
public async Task<UserViewModel> GetUserViewModelAsync(AppUser user)
{
    var comments = await commentService.GetUserCommentsAsync(user.Id);
    
    int totalViews = user.Blogs.Sum(b => b.ViewCount);
    
    var userViewModel = new UserViewModel
    {
        Id = user.Id,
        Username = user.UserName,
        Email = user.Email,
        FullName = user.FullName,
        CreatedDate = user.CreatedDate,
        PhoneNumber = user.PhoneNumber,
        BirthDate = user.BirthDate ?? DateTime.Now,
        ProfilePhotoUrl = user.ProfilePhotoUrl,
        AboutMe = user.AboutMe,
        Blogs = user.Blogs.Select(b => new BlogViewModel
        {
            Id = b.Id,
            Title = b.Title,
            Content = b.Content,
            ImageUrl = b.ImageUrl,
            CreatedAt = b.CreatedAt,
            UpdatedAt = b.UpdatedAt,
            Category = b.Category?.Name,
            CategoryId = b.CategoryId,
            ViewCount = b.ViewCount
        }).ToList(),
        TotalComments = comments.Count,
        TotalViews = totalViews
    };
    return userViewModel;
}
    public UserViewModel GetUserViewModel(AppUser user)
    {
        var userViewModel = new UserViewModel
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            CreatedDate = user.CreatedDate,
            PhoneNumber = user.PhoneNumber,
            BirthDate = user.BirthDate ?? DateTime.Now,
            ProfilePhotoUrl = user.ProfilePhotoUrl,
            AboutMe = user.AboutMe,
            Blogs = user.Blogs.Select(b => new BlogViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Content = b.Content,
                ImageUrl = b.ImageUrl,
                CreatedAt = b.CreatedAt,
                UpdatedAt = b.UpdatedAt
            }).ToList()
        };
        return userViewModel;
    }
    
    public async Task<AppUser> GetUserById(Guid id)
    {
        var user = await _userManager.Users
            .Include(u => u.Blogs)
            .ThenInclude(b => b.Category)
            .FirstOrDefaultAsync(u => u.Id == id);
    
        if (user == null)
        {
            return null;
        }

        return user;
    }
    public async Task<List<UserViewModel>> GetBestWriters(int count=4)
    {
        var users = await _userManager.Users.Include(u => u.Blogs).ToListAsync();
        var bestWriters = users.OrderByDescending(u => u.Blogs.Sum(b=>b.ViewCount)).Take(count).ToList();
        var userViewModels = new List<UserViewModel>();
        foreach (var user in bestWriters)
        {
            userViewModels.Add(await GetUserViewModelAsync(user));
        }

        return userViewModels;
    }
    public async Task<bool> UpdateUserAsync(EditUserViewModel model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
        {
            return false;
        }
    
        user.UserName = model.Username;
        user.Email = model.Email;
        user.FullName = model.FullName;
        user.BirthDate = model.BirthDate;
        user.AboutMe = model.AboutMe;
        user.ProfilePhotoUrl = model.ProfilePhotoUrl;
    
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<List<UserViewModel>> GetAllUsers()
    {
        var users = await _userManager.Users
            .Include(u => u.Blogs)
            .ThenInclude(b => b.Category)
            .ToListAsync();

        var userViewModels = new List<UserViewModel>();
        foreach (var user in users)
        {
            userViewModels.Add(await GetUserViewModelAsync(user));
        }

        return userViewModels;
    }
    public async Task<List<string>> GetRolesAsync(AppUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
    public async Task<bool> AddToRolesAsync(AppUser user, string[] roles)
    {
        var result = await _userManager.AddToRolesAsync(user, roles);
        return result.Succeeded;
    }
    public async Task<bool> RemoveFromRolesAsync(AppUser user, List<string> roles)
    {
        var result = await _userManager.RemoveFromRolesAsync(user, roles);
        return result.Succeeded;
    }
    public async Task<bool> UpdateUserRolesAsync(Guid userId, string[] roles)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return false;
        }
    
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        var result = await _userManager.AddToRolesAsync(user, roles);
        return result.Succeeded;
    }
    public async Task<bool> DeleteAsync(AppUser user)
    {
        try
        {
            
            var userComments = await _commentService.GetUserCommentsAsync(user.Id);

            foreach (var comment in userComments)
            {
                await _commentService.DeleteCommentAsync(comment.Id,user.Id);
            }

            
            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public async Task<bool> CreateRoleAsync(string roleName)
    {
        var role = new AppRole { Name = roleName };
        var result = await _roleManager.CreateAsync(role);
        return result.Succeeded;
    }
    public async Task<PageUserViewModel> GetPaginatedUsersAsync(int page = 1, int pageSize = 10)
    {
        var query = _userManager.Users
            .Include(u => u.Blogs)
            .ThenInclude(b => b.Category);
    
        var totalCount = await query.CountAsync();
    
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var userViewModels = new List<UserViewModel>();
        foreach (var user in users)
        {
            userViewModels.Add(await GetUserViewModelAsync(user));
        }
    
        return new PageUserViewModel
        {
            Users = userViewModels,
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = totalCount
        };
    }
    public Guid? GetCurrentUserId()
    {
        var userId = _userManager.GetUserId(_signInManager.Context.User);
        if (userId == null)
        {
            return null;
        }
        return Guid.Parse(userId);
    }
    public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı" });
        }
    
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }
}