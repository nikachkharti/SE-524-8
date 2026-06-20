using Company524.API.Models.Authentication;
using Company524.API.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace Company524.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// სისტემაში რეგისტრაცია ადმინისტრატორის როლით
        /// </summary>
        [HttpPost("registeradmin")]
        [SwaggerRequestExample(typeof(RegistrationRequestDto), typeof(RegistrationRequestDtoExample))]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegistrationRequestDto model)
        {
            var confirmationBaseUrl = BuildConfirmationBaseUrl(Request);
            var result = await authService.RegisterAdminAsync(model, confirmationBaseUrl);

            var response = new CommonResponse()
            {
                Message = "Admin registered successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.Created),
                Result = result
            };

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// სისტემაში ავტორიზაცია
        /// </summary>
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

        /// <summary>
        /// ელ-ფოსტის საშუალებით ანგარიშის გააქტიურება
        /// </summary>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            await authService.ConfirmEmailAsync(userId, token);

            var response = new CommonResponse()
            {
                Message = "Email confirmed successfully",
                IsSuccess = true,
                HttpStatusCode = Convert.ToInt32(HttpStatusCode.OK)
            };

            return StatusCode(response.HttpStatusCode, response);
        }

        /// <summary>
        /// ტოკენის განახლება
        /// </summary>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            var result = await authService.RefreshTokenAsync(model.RefreshToken);

            return Ok(new CommonResponse
            {
                Message = "Token refreshed successfully",
                IsSuccess = true,
                HttpStatusCode = (int)HttpStatusCode.OK,
                Result = result
            });
        }


        /// <summary>
        /// ტოკენის გაუქმება (გასვლა სისტემიდან)
        /// </summary>
        /// <remarks>
        /// აღნიშნული endpoint ემსახურება სისტემიდან გამოსვლის მექანიზმს.
        ///
        /// ვინაიდან JWT წვდომის ტოკენი უმდგომოა (stateless), მისი გაუქმება გაცემის შემდეგ
        /// შეუძლებელია — ის მოქმედებს ვადის გასვლამდე. თუმცა, განახლების ტოკენის
        /// გაუქმება მონაცემთა ბაზაში კლიენტს ახალი ტოკენების წყვილის მიღების
        /// საშუალებას აძლევს.
        ///
        /// გამოყენების შემთხვევები:
        ///
        /// 1. სტანდარტული გასვლა სისტემიდან
        ///    მომხმარებელი აჭერს "გასვლა" ღილაკს და კლიენტი აგზავნის
        ///    განახლების ტოკენს გასაუქმებლად. მიმდინარე წვდომის ტოკენი
        ///    მოქმედია ვადის გასვლამდე (15 წუთი), შემდეგ კი სესია
        ///    სრულად წყდება.
        ///
        /// 2. ადმინისტრატორის მიერ იძულებითი გასვლა
        ///    ანგარიშის კომპრომეტირების ეჭვის შემთხვევაში შესაძლებელია
        ///    მომხმარებლის ყველა აქტიური სესიის ერთდროულად გაუქმება.
        ///
        /// 3. პაროლის შეცვლა
        ///    საუკეთესო პრაქტიკის შესაბამისად, პაროლის შეცვლისას ყველა
        ///    არსებული სესია უნდა გაუქმდეს, რათა ძველი ტოკენებით
        ///    წვდომა აღარ იყოს შესაძლებელი.
        ///
        /// 4. საეჭვო აქტივობა
        ///    თუ სისტემა აღმოაჩენს განახლების ტოკენის განმეორებით გამოყენებას,
        ///    ტოკენის გაუქმება მიუთითებს პოტენციურ მოპარვაზე.
        ///
        /// შენიშვნა: endpoint მოითხოვს ავტორიზაციას [Authorize], რათა
        /// მხოლოდ სესიის მფლობელმა ან ადმინისტრატორმა შეძლოს გაუქმება.
        /// </remarks>
        /// <param name="model">გასაუქმებელი განახლების ტოკენი</param>
        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequestDto model)
        {
            await authService.RevokeRefreshTokenAsync(model.RefreshToken);

            return Ok(new CommonResponse
            {
                Message = "Token revoked successfully",
                IsSuccess = true,
                HttpStatusCode = (int)HttpStatusCode.OK
            });
        }


        private static string BuildConfirmationBaseUrl(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}/api/auth/confirm-email";
        }
    }
}
