using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace AspNetCoreIdentityApp.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserID { get; set; }

        private readonly UserManager<AppUser> _userManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            var userroles = await _userManager.GetRolesAsync(user);

            var stringbuilder = new StringBuilder();

            userroles.ToList().ForEach(role =>
            {
                stringbuilder.Append($@"<span class='badge bg-secondary mx-1 fw-normal'>{role}</span>");
            });


            output.Content.SetHtmlContent(stringbuilder.ToString());

        }
    }
}
