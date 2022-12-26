using NewsParser.Application.Authentication.Login;
using NewsParser.Application.Authentication.Register;
using NewsParser.Contracts.Authentication;

namespace NewsParser.Application.Authentication.Services;

public interface IAuthService
{
    Task<AuthResult> Login(LoginRequest request);
    Task<AuthResult> Register(RegisterRequest request);
}