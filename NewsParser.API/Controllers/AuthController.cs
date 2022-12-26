using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsParser.Application.Authentication.Login;
using NewsParser.Application.Authentication.Register;
using NewsParser.Application.Authentication.Services;
using NewsParser.Contracts.Authentication;

namespace NewsParser.API.Controllers;

[Route("auth/[action]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<AuthResult> Register([FromBody]RegisterRequest request)
        => await _authService.Register(request);

    [HttpPost]
    public async Task<AuthResult> Login([FromBody]LoginRequest request)
        => await _authService.Login(request);
}