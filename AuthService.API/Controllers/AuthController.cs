using AuthService.API.Application.Commands;
using AuthService.API.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _mediator.Send(new LoginUserCommand(request.Email, request.Password));
        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenRequest request)
    {
        var result = await _mediator.Send(new RefreshTokenCommand(request.Token));
        return Ok(result);
    }

    [HttpPost("revoke")]
    public async Task<IActionResult> Revoke(RefreshTokenRequest request)
    {
        await _mediator.Send(new RevokeRefreshTokenCommand(request.Token));
        return Ok();
    }
}
