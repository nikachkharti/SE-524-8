using Company524.API.Models.Authentication;

namespace Company524.API.Service.Contracts
{
    public interface IAuthService
    {
        Task<string> RegisterAdminAsync(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    }
}
