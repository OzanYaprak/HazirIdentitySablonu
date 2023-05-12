using Microsoft.AspNetCore.Identity;

namespace AspNetCoreIdentityApp.Localizations
{
    //JQERY VALIDASYONLARINI EZMEK İÇİN YAZILDI

    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Code = "DuplicateUserName", Description = $"{userName} başka bir kullanıcı tarafından alınmıştır." };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = "DuplicateEmail", Description = $"{email} başka bir kullanıcı tarafından alınmıştır." };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError { Code = "PasswordTooShort", Description = "Girmiş olduğunuz şifreniz en az 8 karakter olmalıdır." };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError { Code = "PasswordRequiresNonAlphanumeric", Description = "Şifreniz en az bir alfasayısal olmayan karakter içermelidir." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError { Code = "PasswordRequiresUpper", Description = "Şifreniz en az bir büyük harf içermelidir." };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError { Code = "PasswordRequiresLower", Description = "Şifreniz en az bir küçük harf içermelidir." };
        }
    }
}
