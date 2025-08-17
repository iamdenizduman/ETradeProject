namespace AuthService.Application.Features.Users.RegisterUser
{
    public class RegisterUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
    }
}
