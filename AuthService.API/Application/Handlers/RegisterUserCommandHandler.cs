using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using AuthService.API.Domain.Entities;
using AuthService.API.Infrastructure.Auth;
using AuthService.API.Infrastructure.Persistence;
using BuildingBlocks.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Application.Handlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RefreshTokenResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtProvider _jwtProvider;

    public RegisterUserCommandHandler(ApplicationDbContext context, IJwtProvider jwtProvider)
    {
        _context = context;
        _jwtProvider = jwtProvider;
    }

    public async Task<RefreshTokenResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Users.AnyAsync(u => u.Email == request.Email);
        if (exists)
            throw new Exception("User already exists.");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        var refreshToken = RefreshTokenGenerator.GenerateToken();
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        var token = _jwtProvider.GenerateToken(user);
        return new RefreshTokenResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken
        };
    }
}
