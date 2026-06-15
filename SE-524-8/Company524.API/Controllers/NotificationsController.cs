using Company524.API.Models.Notification;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Company524.API.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    public class NotificationsController(IEmailService emailService) : ControllerBase
    {
        [HttpPost("send-email")]
        [SwaggerRequestExample(typeof(SendEmailRequest), typeof(SendEmailRequestDtoExample))]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            var emailSent = await emailService.Send(request.to, request.subject, request.body);
            return emailSent.success ? Ok(emailSent) : BadRequest(emailSent);
        }
    }
}
