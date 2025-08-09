namespace AuthService.Application.Features.Users.LoginUser
{
    public class LoginUserResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}