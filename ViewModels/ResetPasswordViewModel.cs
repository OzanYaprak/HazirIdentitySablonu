using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.ViewModels
{
    public class ResetPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen yeni bir şifre giriniz.")]
        [Display(Name = "Yeni Şifre: ")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler uyumsuz")]
        [Required(ErrorMessage = "Lütfen şifrenizin tekrarını giriniz.")]
        [Display(Name = "Yeni Şifre Tekrar: ")]
        public string PasswordConfirm { get; set; }
    }
}
