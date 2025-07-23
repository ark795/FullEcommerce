using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using AuthService.API.Domain.Entities;
using AuthService.API.Infrastructure.Auth;
using AuthService.API.Infrastructure.Persistence;
using BuildingBlocks.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Application.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, RefreshTokenResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(ApplicationDbContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<RefreshTokenResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new UnauthorizedAccessException();

        var refreshToken = RefreshTokenGenerator.GenerateToken();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        await _context.SaveChangesAsync(cancellationToken);
        var accessToken = _jwtProvider.GenerateToken(user);

        return new RefreshTokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
