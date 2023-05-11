using AspNetCoreIdentityApp.ViewModels;

namespace AspNetCoreIdentityApp.Services
{
    public interface IEmailService
    {
        Task SendPasswordResetEmail(string passwordResetLink, string receiver);
    }
}
