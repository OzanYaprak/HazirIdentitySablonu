using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Areas.Admin.ViewModels
{
    public class AddRoleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ünvan adı girin.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "Ünvan adı min. {2} maks. {1} karakter olabilir.")]
        public string RoleName { get; set; }
    }
}
