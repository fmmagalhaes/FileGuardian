using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Services.Interfaces;

public interface IGroupService
{
    Task<int> CreateGroupAsync(Group group);
    Task AddUsersToGroupAsync(int groupId, List<int> userIds);
    Task<Group> GetGroupAsync(int id);
    Task<List<Group>> GetGroupsAsync();
}