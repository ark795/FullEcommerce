using System.Reflection.Metadata;

namespace BuildingBlocks.Application.Abstractions;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
