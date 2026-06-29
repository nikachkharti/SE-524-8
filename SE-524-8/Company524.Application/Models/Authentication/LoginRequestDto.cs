namespace Company524.Application.Models.Authentication
{
    public record LoginRequestDto
    {
        public string UserName { get; set; } // Email
        public string Password { get; set; }
    }
}
