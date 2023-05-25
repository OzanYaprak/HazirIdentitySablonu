using AspNetCoreIdentityApp.Core.Models;
using AspNetCoreIdentityApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreIdentityApp.ClaimProvider
{
    //AUTHORIZE ATTRIBUTE'U OLAN CONTROLLER'DA AŞAĞIDAKİ CLAIM PROVIDER ÇALIŞACAK.
    public class UserClaimProvider : IClaimsTransformation
    {

        private readonly UserManager<AppUser> _userManager;

        public UserClaimProvider(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var identityuser = principal.Identity as ClaimsIdentity;
            var currentuser = await _userManager.FindByNameAsync(identityuser.Name);


            //eğer kullanıcının city'si null ise veya boş ise tekrardan principal'ı dön
            if (String.IsNullOrEmpty(currentuser.City))
            {
                return principal;
            }


            //eğer kullanıcının city'si null veya boş değil ise aşağıda city adında bir claim nesnesi ekle
            if (principal.HasClaim(a => a.Type != "City"))
            {
                Claim cityclaim = new Claim("City", currentuser.City);

                identityuser.AddClaim(cityclaim);
            }



            //Yukarıdaki if bloğundan sonra güncellenmiş ve city claim'i eklenmiş principal olarak dön
            return principal;

        }
    }
}
