using NewsParser.Application.Authentication.Common.Errors;
using NewsParser.Application.Authentication.Login;
using NewsParser.Application.Authentication.Register;
using NewsParser.Application.Common.Errors;
using NewsParser.Contracts.Authentication;
using NewsParser.Domain.Authentication.Models;

namespace NewsParser.Infrastructure.Authentication.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<AuthResult> Login(LoginRequest request)
    {
        var (email, password) = request;

        if (await _userRepository.GetUserByEmailAsync(email) is not User user)
            throw new InvalidEmailException();

        if (user.Password != password)
            throw new InvalidPasswordException();
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        return new AuthResult(
            user,
            token
        );
    }
    
    public async Task<AuthResult> Register(RegisterRequest request)
    {
        var (firstName, lastName, email, password) = request;

        if (await _userRepository.GetUserByEmailAsync(email) is not null)
            throw new DuplicateEmailException();

        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        var id = await _userRepository.AddAsync(user);

        user.Id = id;

        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthResult(
            user,
            token
        );
    }
    
}