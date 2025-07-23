using AuthService.API.Domain.Entities;

namespace BuildingBlocks.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
