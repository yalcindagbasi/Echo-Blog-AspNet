using BlogAspNet.Web.Models.Services.ViewModels;

public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public string ProfilePhotoUrl { get; set; } = "https://upload.wikimedia.org/wikipedia/commons/7/7c/Profile_avatar_placeholder_large.png?20150327203541";
        public string? AboutMe { get; set; } = "About me"; 
        public DateTime CreatedDate { get; set; } 
        public List<BlogViewModel> Blogs { get; set; } = new();
        public int TotalComments { get; set; } = 0;
        public int TotalViews { get; set; } = 0;
    }