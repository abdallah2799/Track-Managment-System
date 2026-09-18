using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return Ok(response);
    }
}
