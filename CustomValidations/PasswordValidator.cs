using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password)
        {
            var errors = new List<IdentityError>();

            // Yaygın şifreler kontrolü
            var commonPasswords = new List<string> { "password", "12345678", "qwerty", "abcdefgh", "password123","şifre123","şifre321", "passwordd", "passworddd" };



            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code="PasswordContainsUserName",
                    Description="Kullanıcı adı şifre belirlemede kullanılamaz."
                });
            }

            //if (password.Length < 8)
            //{
            //    errors.Add(new IdentityError()
            //    {
            //        Code = "PasswordTooShort",
            //        Description = "Şifreniz en az 8 karakter uzunluğunda olmalıdır."
            //    });
            //}

            // Yaygın şifreler kontrolü
            if (commonPasswords.Contains(password.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordTooCommon",
                    Description = "Şifreniz çok yaygın bir şifre olduğu için kabul edilemez."
                });
            }




            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
