using NewsParser.Domain.Authentication.Models;

namespace NewsParser.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}