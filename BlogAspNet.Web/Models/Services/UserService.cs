using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Models.Services;

public class UserService(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    SignInManager<AppUser> signInManager)
    : IUserService
{
    private readonly RoleManager<AppRole> _roleManager = roleManager;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly SignInManager<AppUser> _signInManager = signInManager;
    
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
            throw new Exception("User not found");
        }

        return user;
    }
    public async Task<List<UserViewModel>> GetBestWriters()
    {
        var users = await _userManager.Users.Include(u => u.Blogs).ToListAsync();
        var bestWriters = users.OrderByDescending(u => u.Blogs.Count).Take(4).ToList();
        var userViewModels = new List<UserViewModel>();
        foreach (var user in bestWriters)
        {
            userViewModels.Add(GetUserViewModel(user));
        }

        return userViewModels;
    }
    
    
}