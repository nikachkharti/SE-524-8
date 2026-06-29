namespace Company524.Application.Models.Authentication
{
    public record RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
