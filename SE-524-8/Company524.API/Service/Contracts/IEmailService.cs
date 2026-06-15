using Company524.API.Models.Notification;

namespace Company524.API.Service.Contracts
{
    public interface IEmailService
    {
        Task<SendEmailResponse> Send(string to, string subject, string body);
    }
}
