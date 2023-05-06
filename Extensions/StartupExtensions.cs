using AspNetCoreIdentityApp.CustomValidations;
using AspNetCoreIdentityApp.Data;
using AspNetCoreIdentityApp.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AspNetCoreIdentityApp.Extensions
{
    public static class StartupExtensions
    {
        public static void IdentityExtensions(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;


            }).AddUserValidator<UserValidator>().AddPasswordValidator<PasswordValidator>().AddEntityFrameworkStores<AppDBContext>();
        }
    }
}
