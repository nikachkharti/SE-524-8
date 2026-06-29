namespace Company524.Application.Models.Notification
{
    public record SendEmailResponse(bool success, string message, Exception error = null);
}
