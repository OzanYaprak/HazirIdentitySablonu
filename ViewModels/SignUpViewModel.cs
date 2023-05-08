using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.ViewModels
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage ="Kullanıcı adı boş bırakılamaz.")]
        [Display(Name ="Kullanıcı Adı: ")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email giriniz.")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [Display(Name = "Email: ")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon: ")]
        [RegularExpression(@"^\+?\d{1,3}?[-.\s]?\(?\d{3}\)?[-.\s]?\d{3}[-.\s]?\d{4}$", ErrorMessage = "Geçerli bir telefon numarası giriniz.")] //regex
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Lütfen bir şifre giriniz.")]
        [Display(Name = "Şifre: ")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage ="Şifreler uyumsuz")]
        [Required(ErrorMessage = "Lütfen şifrenizin tekrarını giriniz.")]
        [Display(Name = "Şifre Tekrar: ")]
        public string PasswordConfirm { get; set; }
    }
}
