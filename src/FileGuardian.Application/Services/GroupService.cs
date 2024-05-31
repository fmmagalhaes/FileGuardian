using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Exceptions;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace FileGuardian.Application.Services;

public class GroupService(
    IGroupRepository groupRepository,
    IGroupUserRepository groupUserRepository,
    ILogger<GroupService> logger) : IGroupService
{
    public async Task<int> CreateGroupAsync(Group group)
    {
        var groupWithName = await groupRepository.GetByNameAsync(group.Name);
        if (groupWithName != null)
        {
            throw new NameAlreadyInUseException($"Group name {group.Name} is already in use");
        }
        return await groupRepository.AddAsync(group);
    }

    public async Task AddUsersToGroupAsync(int groupId, List<int> userIds)
    {
        var groupUsers = userIds.Select(userId => new GroupUser { GroupId = groupId, UserId = userId });
        await groupUserRepository.BulkUpsertAsync(groupUsers);

        logger.LogInformation("Added {UserCount} users to group {GroupId}", userIds.Count, groupId);
    }

    public async Task<Group> GetGroupAsync(int id)
    {
        var group = await groupRepository.GetByIdAsync(id);
        return group ?? throw new NotFoundException($"Group with id {id} was not found");
    }

    public async Task<List<Group>> GetGroupsAsync()
    {
        return await groupRepository.GetGroupsAsync();
    }
}