using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreIdentityApp.Requirements
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {
    }


    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {
            //Böyle bir claim yok ise hata döndür
            if (!context.User.HasClaim(a => a.Type == "ExchangeExpireDate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var exchangeExpireClaim = context.User.FindFirst("ExchangeExpireDate");

            //Eğer claim süresi, istek yapılan zamanı geçmiş ise..
            if (DateTime.Now > Convert.ToDateTime(exchangeExpireClaim.Value))
            {
                context.Fail();
                return Task.CompletedTask;
            }


            context.Succeed(requirement);

            return Task.CompletedTask;


        }
    }
}
