using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Models
{
    public class AppUser:IdentityUser
    {
        public string City { get; set; }
        public string Picture { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}
