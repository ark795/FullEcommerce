using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace BuildingBlocks.Infrastructure.Auth;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        // TODO: Custom token extraction/validation if needed
        await _next(context);
    }
}
