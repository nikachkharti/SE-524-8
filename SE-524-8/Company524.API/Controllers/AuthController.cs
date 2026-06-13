using Company524.API.Models.Authentication;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Company524.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("registeradmin")]
        [SwaggerRequestExample(typeof(RegistrationRequestDto), typeof(RegistrationRequestDtoExample))]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            var result = await authService.RegisterAdminAsync(model);

            var response = new CommonResponse()
            {
                Message = "Admin registered successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }


        [HttpPost("login")]
        [SwaggerRequestExample(typeof(LoginRequestDto), typeof(LoginRequestDtoExample))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var result = await authService.LoginAsync(model);

            var response = new CommonResponse()
            {
                Message = "Successful authorization",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }
    }
}
