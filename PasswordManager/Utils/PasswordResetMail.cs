using Microsoft.Extensions.Options;
using PasswordManager.OptionModels;
using System.Net;
using System.Net.Mail;

namespace PasswordManager.Utils
{
    public class PasswordResetMail
    {
        private readonly EmailSettings emailSettings;

        public PasswordResetMail(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string ToEmail)
        {
            var smptClient = new SmtpClient
            {
                Host = emailSettings.Host,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Port = 587,
                Credentials = new NetworkCredential(emailSettings.Email, emailSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings.Email)
            };
            mailMessage.To.Add(ToEmail);

            mailMessage.Subject = "Password Manager | Şifre Sıfırlama Bağlantısı";
            mailMessage.Body = @$"
                  <h4>Şifrenizi yenilemek için aşağıdaki linkte tıklayınız.</h4>
                  <p><a href='{resetPasswordEmailLink}'>şifre yenileme link</a></p>";

            mailMessage.IsBodyHtml = true;


            await smptClient.SendMailAsync(mailMessage);
        }
    }
}
