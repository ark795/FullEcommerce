using System.Security.Cryptography;

namespace AuthService.API.Infrastructure.Auth;

public class RefreshTokenGenerator
{
    public static string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}
