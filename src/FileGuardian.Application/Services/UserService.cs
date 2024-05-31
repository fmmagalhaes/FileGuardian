using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Exceptions;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<int> CreateUserAsync(User user)
    {
        var userWithName = await userRepository.GetByNameAsync(user.Name);
        if (userWithName != null)
        {
            throw new NameAlreadyInUseException($"Username {user.Name} is already in use");
        }
        return await userRepository.AddAsync(user);
    }

    public async Task<User> GetUserAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user ?? throw new NotFoundException($"User with id {id} was not found");
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await userRepository.GetUsersAsync();
    }
}