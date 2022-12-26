using NewsParser.Domain.Authentication.Models;

namespace NewsParser.Application.Common.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<int> AddAsync(User user);
}