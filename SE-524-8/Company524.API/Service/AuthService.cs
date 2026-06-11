using Company524.API.Data;
using Company524.API.Exceptions;
using Company524.API.Models.Authentication;
using Company524.API.Service.Contracts;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company524.API.Service
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private const string _adminRole = "Admin";

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }


        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower().Trim() == loginRequestDto.UserName.ToLower().Trim());

            if (user == null)
                throw new BadRequestException("User with provided credentials not found");

            if (!user.LockoutEnabled)
                throw new UnauthorizedException("Unable to sign in with locked account");

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!isValid)
                throw new BadRequestException("Username or Password is incorrect");

            //If user was found generate token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            return new LoginResponseDto() { AccessToken = token };
        }

        public async Task<string> RegisterAdminAsync(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(registrationRequestDto);

            //Register user
            IdentityResult result = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.Email.ToLower() == registrationRequestDto.Email.ToLower());

                //If user registers, assign to role
                if (userToReturn != null)
                {
                    //If role not exists, insert new role
                    if (!await _roleManager.RoleExistsAsync(_adminRole))
                        await _roleManager.CreateAsync(new IdentityRole(_adminRole));

                    await _userManager.AddToRoleAsync(userToReturn, _adminRole);
                }

                return userToReturn.Id;
            }
            else
            {
                throw new BadRequestException(result.Errors.FirstOrDefault().Description);
            }
        }
    }
}
