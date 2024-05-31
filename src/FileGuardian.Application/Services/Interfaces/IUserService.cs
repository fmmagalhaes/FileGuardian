using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Services.Interfaces;

public interface IUserService
{
    Task<int> CreateUserAsync(User user);
    Task<User> GetUserAsync(int id);
    Task<List<User>> GetUsersAsync();
}