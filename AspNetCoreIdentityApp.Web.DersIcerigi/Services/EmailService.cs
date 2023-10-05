﻿using AspNetCoreIdentityApp.Web.DersIcerigi.OptionModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreIdentityApp.Web.DersIcerigi.Services
{
    public class EmailService : IEmailService
    {

        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host =_emailSettings.Host;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Port = 587;
            smtpClient.Credentials=new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
            smtpClient.EnableSsl = true;

            var mailMessage= new MailMessage();

            mailMessage.From=new MailAddress(_emailSettings.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Localhost | Şifre sıfırlama linki";
            mailMessage.Body = @$"<h4>Şifrenizi yenilemek için aşağıdaki linke tıklayınız</h4>
                                  <p><a href='{resetPasswordEmailLink}'> Şifre yenileme link</a></p>";
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
