using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Abstractions;

public interface IUserRepository
{
    Task<int> AddAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByNameAsync(string name);
    Task<List<User>> GetUsersAsync();
}
