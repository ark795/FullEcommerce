namespace AuthService.API.Application.DTOs;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
}
