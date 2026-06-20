using Company524.API.Models.Authentication;

namespace Company524.API.Service.Contracts
{
    public interface IAuthService
    {
        Task<string> RegisterAdminAsync(RegistrationRequestDto registrationRequestDto, string accountConfirmationUrl = null);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task ConfirmEmailAsync(string userId, string token);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
