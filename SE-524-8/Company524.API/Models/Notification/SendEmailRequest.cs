namespace Company524.API.Models.Notification
{
    public record SendEmailRequest(string to, string subject, string body);
}
