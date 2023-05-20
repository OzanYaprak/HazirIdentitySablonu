using AspNetCoreIdentityApp.CustomValidations;
using AspNetCoreIdentityApp.Data;
using AspNetCoreIdentityApp.Localizations;
using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
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

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 5;

            })
                .AddErrorDescriber<LocalizationIdentityErrorDescriber>()
                .AddUserValidator<UserValidator>()
                .AddPasswordValidator<PasswordValidator>()
                .AddEntityFrameworkStores<AppDBContext>()
                .AddDefaultTokenProviders(); //şifremi unuttum için identity kütüp. ile gelen default token üretmek için yazıldı.




            //şifre sıfırlamada üretilecek token ayarları
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                //şifre sıfırlamada üretilen tokenın süresi 1 saat olarak ayarlandı.
                options.TokenLifespan = TimeSpan.FromHours(1);
            });

        }






        public static void CookieExtensions(this IServiceCollection services)
        {
            //Cookie işlemleri
            services.ConfigureApplicationCookie(options =>
            {
                var cookieBuilder = new CookieBuilder();

                //cookie isim verme
                cookieBuilder.Name = "IdentityAppCookie";

                //kullanıcı girişi için oluşturulan path
                options.LoginPath = new PathString("/Home/SignIn");

                //kullanıcı çıkışı için oluşturulan path
                options.LogoutPath = new PathString("/Member/Logout");

                //AccessDenied sayfası için path oluşturuldu
                options.AccessDeniedPath = new PathString("/Member/AccessDenied");

                options.Cookie = cookieBuilder;

                //cookienin kaç gün tutulacağı
                options.ExpireTimeSpan = TimeSpan.FromDays(90);

                //kullanıcı siteye her giriş yaptığında cookie süresi otomatik olarak +90 gün ilave edilip uzatılacak. false bırakılırsa 90 gün sonunda cookie otomatik silinecek.
                options.SlidingExpiration = true; // 
            });
        }
    }
}
