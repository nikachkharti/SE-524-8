namespace Company524.API.Models.Notification
{
    public record SendEmailResponse(bool success, string message, Exception error = null);
}
