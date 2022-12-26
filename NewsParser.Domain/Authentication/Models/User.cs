using System.ComponentModel.DataAnnotations;

namespace NewsParser.Domain.Authentication.Models;

public record User
{
    public int Id { get; set; }

    [MaxLength(255)] public required string FirstName { get; set; }

    [MaxLength(255)] public required string LastName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email.")] public required string Email { get; set; }

    [MaxLength(255)] public required string Password { get; set; }
    
    public void Deconstruct(out string firstName, out string lastName, out string email,
        out string password)
    {
        firstName = FirstName;
        lastName = LastName;
        email = Email;
        password = Password;
    }
}