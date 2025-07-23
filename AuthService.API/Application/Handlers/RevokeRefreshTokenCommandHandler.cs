using AuthService.API.Application.Commands;
using AuthService.API.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Application.Handlers;

public class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Unit>
{
    private readonly ApplicationDbContext _context;

    public RevokeRefreshTokenCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.Token == request.Token, cancellationToken);

        if (token is null)
            throw new Exception("Token not found");

        token.IsRevoked = true;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
