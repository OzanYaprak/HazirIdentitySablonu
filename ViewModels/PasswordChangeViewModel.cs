using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen eski şifrenizi giriniz.")]
        [Display(Name = "Şifreniz: ")]
        [MinLength(8,ErrorMessage ="Eski şifreniz en az 8 karakter olmak zorundadır")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen yeni bir şifre giriniz.")]
        [MinLength(8, ErrorMessage = "Yeni şifreniz en az 8 karakter olmak zorundadır")]
        [Display(Name = "Yeni Şifreniz: ")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler uyumsuz")]
        [Required(ErrorMessage = "Lütfen yeni şifrenizin tekrarını giriniz.")]
        [MinLength(8, ErrorMessage = "Yeni şifreniz en az 8 karakter olmak zorundadır")]
        [Display(Name = "Yeni Şifre Tekrar: ")]
        public string NewPasswordConfirm { get; set; }
    }
}
