using Company524.Application.Contracts.Service;
using Company524.Application.Exceptions;
using Company524.Application.Models.Authentication;
using Company524.Domain.Entities;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Company524.Application.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private const string _adminRole = "Admin";
        private const string _supplierRole = "Supplier";
        private const string _confirmEmailTitle = "Email Confirm";

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IEmailService emailService,
            IJwtTokenGenerator jwtTokenGenerator,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(x =>
                    x.UserName.ToLower().Trim() ==
                    loginRequestDto.UserName.ToLower().Trim());

            if (user == null)
                throw new BadRequestException("User with provided credentials not found");

            if (!user.EmailConfirmed)
                throw new UnauthorizedException("Unable to sign in with locked account. Check your email and activate account first");

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!isValid)
                throw new BadRequestException("Username or Password is incorrect");

            var roles = await _userManager.GetRolesAsync(user);

            return await GenerateTokenPairAsync(user, roles);
        }
        public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken)
        {
            // Load token with its user in one query
            var existing = await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (existing == null)
                throw new BadRequestException("Invalid refresh token");

            if (!existing.IsActive)
                throw new UnauthorizedException(
                    existing.IsExpired ? "Refresh token has expired" : "Refresh token has been revoked");

            // Revoke the old token (rotate)
            existing.RevokedAt = DateTimeOffset.Now;

            var roles = await _userManager.GetRolesAsync(existing.User);
            var response = await GenerateTokenPairAsync(existing.User, roles);

            // Persist the revocation + new token atomically
            await _context.SaveChangesAsync();

            return response;
        }
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

            if (existing == null)
                throw new BadRequestException("Invalid refresh token");

            if (!existing.IsActive)
                throw new BadRequestException("Token is already inactive");

            existing.RevokedAt = DateTimeOffset.Now;
            await _context.SaveChangesAsync();
        }
        public async Task ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                throw new BadRequestException(
                    "User id and token are required parameters for email confirmation");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new BadRequestException("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
            await _userManager.ResetAccessFailedCountAsync(user);
        }
        public Task<string> RegisterSupplierAsync(
            RegistrationRequestDto registrationRequestDto,
            string accountConfirmationUrl = null)
        {
            return RegisterUserAsync(
                registrationRequestDto,
                _supplierRole,
                accountConfirmationUrl);
        }
        public Task<string> RegisterAdminAsync(
            RegistrationRequestDto registrationRequestDto,
            string accountConfirmationUrl = null)
        {
            return RegisterUserAsync(
                registrationRequestDto,
                _adminRole,
                accountConfirmationUrl);
        }




        //-----Private helpers-----
        private async Task<LoginResponseDto> GenerateTokenPairAsync(ApplicationUser user, IList<string> roles)
        {
            var accessToken = _jwtTokenGenerator.GenerateToken(user, roles);

            var refreshToken = new RefreshToken
            {
                Token = _jwtTokenGenerator.GenerateRefreshToken(),
                UserId = user.Id,
                CreatedAt = DateTimeOffset.Now,
                ExpiresAt = DateTimeOffset.Now.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpiryDays"]))
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token
            };
        }
        private static string BuildAccountConfirmationUrl(string accountConfirmationUrl, ApplicationUser userToReturn, string token)
        {
            return $"{accountConfirmationUrl}" +
                   $"?userId={Uri.EscapeDataString(userToReturn.Id)}" +
                   $"&token={Uri.EscapeDataString(token)}";
        }
        private static string EmailConfirmationBody(string confirmationUrl)
        {
            return $@"
                <h2>Account Activation</h2>
                <p>Your administrator account has been created.</p>
                <p>Please click the link below to activate your account:</p>
                <p>
                    <a href=""{confirmationUrl}"">
                        Activate Account
                    </a>
                </p>";
        }
        private async Task EnsureRoleExistsAsync(string role)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
        private async Task<string> RegisterUserAsync(
            RegistrationRequestDto registrationRequestDto,
            string role,
            string accountConfirmationUrl = null)
        {
            var user = _mapper.Map<ApplicationUser>(registrationRequestDto);

            user.EmailConfirmed = false;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;

            var result = await _userManager.CreateAsync(
                user,
                registrationRequestDto.Password);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.First().Description);

            await EnsureRoleExistsAsync(role);

            await _userManager.AddToRoleAsync(user, role);

            var confirmationToken =
                await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationUrl = BuildAccountConfirmationUrl(
                accountConfirmationUrl,
                user,
                confirmationToken);

            var emailResponse = await _emailService.Send(
                user.Email,
                _confirmEmailTitle,
                EmailConfirmationBody(confirmationUrl));

            if (!emailResponse.success)
                throw new InternalServerException(emailResponse.error.Message);

            return user.Id;
        }
    }
}