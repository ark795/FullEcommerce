using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using AuthService.API.Infrastructure.Auth;
using AuthService.API.Infrastructure.Persistence;
using BuildingBlocks.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Application.Handlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public RefreshTokenCommandHandler(ApplicationDbContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var oldToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.Token && !rt.IsRevoked, cancellationToken);

        if (oldToken == null || oldToken.ExpiresAt < DateTime.UtcNow)
            throw new UnauthorizedAccessException();

        oldToken.IsRevoked = true;

        var newRefreshToken = RefreshTokenGenerator.GenerateToken();
        oldToken.User.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _context.SaveChangesAsync(cancellationToken);
        var accessToken = _jwtProvider.GenerateToken(oldToken.User);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        };
    }
}
