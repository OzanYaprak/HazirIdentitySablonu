using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Configuration;

namespace AspNetCoreIdentityApp.TagHelpers
{
    public class UserPictureThumbnailTagHelper : TagHelper
    {
        public string PictureURL { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            if (string.IsNullOrEmpty(PictureURL))
            {
                output.Attributes.SetAttribute("src", "/userpictures/default_profile_picture.png");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpictures/{PictureURL}");
            }
        }
    }
}
