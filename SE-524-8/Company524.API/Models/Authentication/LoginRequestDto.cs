namespace Company524.API.Models.Authentication
{
    public record LoginRequestDto
    {
        public string UserName { get; set; } // Email
        public string Password { get; set; }
    }
}
