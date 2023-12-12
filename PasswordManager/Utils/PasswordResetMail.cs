using PasswordManager.OptionModels;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

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
            var smptClient = new SmtpClient();

            smptClient.Host = emailSettings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(emailSettings.Email, emailSettings.Password);
            smptClient.EnableSsl = true;

            var mailMessage = new MailMessage();

            mailMessage.From = new MailAddress(emailSettings.Email);
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
