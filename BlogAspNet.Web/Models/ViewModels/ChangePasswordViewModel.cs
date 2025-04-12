using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.Web.Models.ViewModels;

public class ChangePasswordViewModel
{
    [Required(ErrorMessage = "Mevcut şifre gereklidir")]
    [DataType(DataType.Password)]
    [Display(Name = "Mevcut Şifre")]
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "Yeni şifre gereklidir")]
    [StringLength(100, ErrorMessage = "Şifre en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Yeni Şifre (Tekrar)")]
    [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
    public string ConfirmPassword { get; set; }
}