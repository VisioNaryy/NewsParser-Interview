using NewsParser.Domain.Authentication.Models;

namespace NewsParser.Contracts.Authentication;

public record AuthResult(
    User User,
    string Token);