using MediatR;

namespace AuthService.API.Application.Commands;

public record RevokeRefreshTokenCommand(string Token) : IRequest<Unit>;
