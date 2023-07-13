using HireHub.Web.Services.Data.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using HireHub.Data;

namespace HireHub.Web.Services.Data
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task SendEmailAsync(string? email, string? subject, string? message, string smtpService, int smtpServicePort, string smtpUser, string smtpPassword, string mailMessageFrom)
        {
            var client = new SmtpClient(smtpService, smtpServicePort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage(mailMessageFrom, email, subject, message);
            mailMessage.IsBodyHtml = true;

            await client.SendMailAsync(mailMessage);
        }
    }
}
