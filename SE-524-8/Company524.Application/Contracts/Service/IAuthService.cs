using Company524.Application.Models.Authentication;

namespace Company524.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<string> RegisterAdminAsync(
            RegistrationRequestDto registrationRequestDto,
            string accountConfirmationUrl = null
        );
        Task<string> RegisterSupplierAsync(
            RegistrationRequestDto registrationRequestDto,
            string accountConfirmationUrl = null
        );
        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
        Task ConfirmEmailAsync(string userId, string token);
    }
}
