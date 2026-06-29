using Company524.Application.Models.Notification;

namespace Company524.Application.Contracts.Service
{
    public interface IEmailService
    {
        Task<SendEmailResponse> Send(string to, string subject, string body);
    }
}
