namespace NewsParser.Application.Authentication.Register;

public record RegisterRequest
    (string FirstName, string LastName, string Email, string Password);