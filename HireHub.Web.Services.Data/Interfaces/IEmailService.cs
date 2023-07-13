using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireHub.Web.Services.Data.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string? email, string? subject, string? message, string smtpService, int smtpServicePort, string smtpUser, string smtpPassword, string mailMessageFrom);
    }
}
