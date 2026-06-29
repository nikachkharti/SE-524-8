namespace Company524.Application.Models.Authentication
{
    public record LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
