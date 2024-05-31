using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileGuardian.Infrastructure.Persistence;

public class UserRepository(FileGuardianDbContext dbContext) : IUserRepository
{
    public async Task<int> AddAsync(User user)
    {
        await dbContext.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return user.Id;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByNameAsync(string name)
    {
        return await dbContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Name == name);
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await dbContext.Users.AsNoTracking().ToListAsync();
    }
}