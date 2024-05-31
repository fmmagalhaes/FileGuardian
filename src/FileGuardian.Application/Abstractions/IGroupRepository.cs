using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Abstractions;

public interface IGroupRepository
{
    Task<int> AddAsync(Group group);
    Task<Group?> GetByIdAsync(int id);
    Task<Group?> GetByNameAsync(string name);
    Task<List<Group>> GetGroupsAsync();
}
