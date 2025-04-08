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
    
    [Required(ErrorMessage = "Şifre alanı zorunludur.")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 3)]
    [DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = null!;
    public DateTime BirthDate { get; set; }  
}