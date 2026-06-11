namespace Company524.API.Models.Authentication
{
    public record RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
