using AspNetCoreIdentityApp.Models;
using AspNetCoreIdentityApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailsettings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _emailsettings = settings.Value;
        }

        public async Task SendPasswordResetEmail(string passwordResetLink, string receiver)
        {
            var smtpClient = new SmtpClient();
            var mailMessage = new MailMessage();

            smtpClient.Host = _emailsettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential(_emailsettings.Email, _emailsettings.Password);
            smtpClient.EnableSsl = true;

            mailMessage.From = new MailAddress(_emailsettings.Email);
            mailMessage.To.Add(receiver);

            mailMessage.Subject = "Identity Şifre Sıfırlama Bağlantısı";
            mailMessage.Body =
                @$" <h3>Merhaba Sayın Kullanıcı</h3>
                    <h4>Şifrenizi yenilemek için aşağıdaki bağlantıya tıklayınız.</h4>
                    <p><a href='{passwordResetLink}'>Tıklayınız.</a></p>";
            
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

    }
}
