using BeachTowelShop.Services.Data;
using BeachTowelShop.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BeachTowelShop.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
       

        public async Task SendEmail( string to, string subject, string html)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_appSettings.EmailFrom));
            
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            using var smtp = new SmtpClient();
           await smtp.ConnectAsync(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
           await smtp.AuthenticateAsync(_appSettings.SmtpUser, _appSettings.SmtpPass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
