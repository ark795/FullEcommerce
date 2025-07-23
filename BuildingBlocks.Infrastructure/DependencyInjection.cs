using BuildingBlocks.Application.Abstractions;
using BuildingBlocks.Infrastructure.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBuildingBlocks(this IServiceCollection services)
    {
        services.AddScoped<IJwtProvider, JwtProvider>();
        return services;
    }
}
