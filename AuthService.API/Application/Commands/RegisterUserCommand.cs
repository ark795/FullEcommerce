using AuthService.API.Application.DTOs;
using MediatR;

namespace AuthService.API.Application.Commands;

public record RegisterUserCommand(string Email, string Password) : IRequest<RefreshTokenResponse>;
