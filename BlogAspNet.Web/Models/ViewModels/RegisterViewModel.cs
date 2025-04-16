using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.Web.Models.Services.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur")]
    [DataType(DataType.Text)]
    [Display(Name = "Kullanıcı adı")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "E-posta alanı zorunludur")]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = null!;
    
    [Required(ErrorMessage = "Ad alanı zorunludur")]
    [Display(Name = "Adı Soyadı")]
    [StringLength(100, ErrorMessage = "Adı Soyadı en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 3)]
    public string FullName { get; set; } = null!;
    
    [Display(Name = "Hakkımda")]
    [StringLength(4000)]
    public string? AboutMe { get; set; }
    
    [Required(ErrorMessage = "Şifre alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Doğum tarihi alanı zorunludur.")]
    public DateTime BirthDate { get; set; }
    [StringLength(400)]
    public string? ProfilePhotoUrl { get; set; } 
    public IFormFile? ProfilePhotoFile { get; set; }

}