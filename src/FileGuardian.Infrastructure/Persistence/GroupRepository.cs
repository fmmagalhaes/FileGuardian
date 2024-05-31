using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileGuardian.Infrastructure.Persistence;

public class GroupRepository(FileGuardianDbContext dbContext) : IGroupRepository
{
    public async Task<int> AddAsync(Group group)
    {
        await dbContext.AddAsync(group);
        await dbContext.SaveChangesAsync();

        return group.Id;
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        return await dbContext.Groups
            .AsNoTracking()
            .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
            .SingleOrDefaultAsync(u => u.Id == id);
    }

    public async Task<Group?> GetByNameAsync(string name)
    {
        return await dbContext.Groups.AsNoTracking().SingleOrDefaultAsync(u => u.Name == name);
    }

    public async Task<List<Group>> GetGroupsAsync()
    {
        return await dbContext.Groups.AsNoTracking().ToListAsync();
    }
}