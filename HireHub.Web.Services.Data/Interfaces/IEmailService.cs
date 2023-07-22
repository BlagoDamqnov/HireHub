namespace HireHub.Web.Services.Data.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string? email, string? subject, string? message, string smtpService, int smtpServicePort, string smtpUser, string smtpPassword, string mailMessageFrom);
    }
}