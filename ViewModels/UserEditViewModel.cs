using AspNetCoreIdentityApp.Models;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        [Display(Name = "Kullanıcı Adı: ")]
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

        [DataType(DataType.Date, ErrorMessage = "Geçersiz tarih formatı.")]
        [Display(Name = "Doğum Tarihi")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}", NullDisplayText = "Tarih bilgisi yok.")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50, ErrorMessage = "Şehir alanı en fazla 50 karakter olabilir.")]
        [RegularExpression(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ\s]+$", ErrorMessage = "Şehir alanı sadece harf ve boşluk içerebilir.")]
        public string City { get; set; }

        [Display(Name = "Profil Resmi")]
        public IFormFile Picture { get; set; }

        [Display(Name = "Cinsiyet")]
        public Gender Gender { get; set; }
    }
}
