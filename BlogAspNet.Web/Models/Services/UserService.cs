using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using BlogAspNet.Web.Models.Services.ViewModels;
using Microsoft.AspNetCore.Identity;

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
            BirthDate = model.BirthDate
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
    
    //get user by id
    public async Task<AppUser> GetUserById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }
    
    
}