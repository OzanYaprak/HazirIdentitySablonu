using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityApp.Areas.Admin.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Lütfen bir ünvan adı girin.")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Ünvan adı min. {2} maks. {1} karakter olabilir.")]
        public string RoleName { get; set; }
    }
}
