using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace AspNetCoreIdentityApp.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors = new List<IdentityError>();

            var isDigit = int.TryParse(user.UserName[0].ToString(), out _);




            if (isDigit)
            {
                errors.Add(new IdentityError()
                {
                    Code="UserNameContainFirstLetterDigit", Description="Kullanıcı adının ilk karakteri sayısal bir karakter içeremez."
                });
            }


            // Kullanıcı adı için gereksinimler kontrolü
            if (user.UserName.Length < 3)
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameTooShort",
                    Description = "Kullanıcı adı en az 3 karakter uzunluğunda olmalıdır."
                });
            }
            else if (user.UserName.Length > 50)
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameTooLong",
                    Description = "Kullanıcı adı en fazla 50 karakter uzunluğunda olmalıdır."
                });
            }
            else if (!Regex.IsMatch(user.UserName, @"^[a-zA-Z0-9]+$"))
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameInvalidCharacters",
                    Description = "Kullanıcı adı yalnızca harf ve sayı içerebilir."
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
