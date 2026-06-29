namespace Company524.Application.Models.Notification
{
    public record SendEmailRequest(string to, string subject, string body);
}
