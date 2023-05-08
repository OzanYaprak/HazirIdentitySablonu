using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.ViewModels
{
    public class SignInViewModel
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email giriniz.")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen bir şifre giriniz.")]
        [Display(Name = "Şifre: ")]
        public string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}
