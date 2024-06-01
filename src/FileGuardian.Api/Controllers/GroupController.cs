using AutoMapper;
using FileGuardian.Api.DTOs;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileGuardian.Api.Controllers;

[Route("api/groups")]
[ApiController]
public class GroupController(IGroupService groupService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Creates a group.
    /// </summary>
    /// <param name="groupRequest"></param>
    /// <returns>The id of the new group.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateGroup([FromBody] GroupDto groupRequest)
    {
        var group = mapper.Map<Group>(groupRequest);
        var groupId = await groupService.CreateGroupAsync(group);
        return Ok(groupId);
    }

    /// <summary>
    /// Adds users to a group.
    /// </summary>
    /// <param name="id">The id of the group.</param>
    /// <param name="userIds">The list of userIds to be added to the group.</param>
    [HttpPost("{id}/users")]
    public async Task<ActionResult<int>> AddUsersToGroup(int id, [FromBody] List<int> userIds)
    {
        await groupService.AddUsersToGroupAsync(id, userIds);
        return Ok();
    }

    /// <summary>
    /// Gets the details of a group.
    /// </summary>
    /// <param name="id">The id of the group.</param>
    /// <returns>The group details, including list of users.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetGroupResponse>> GetGroup(int id)
    {
        var group = await groupService.GetGroupAsync(id);
        var groupResponse = mapper.Map<GetGroupResponse>(group);
        return Ok(groupResponse);
    }

    /// <summary>
    /// Gets a list of all groups.
    /// </summary>
    /// <returns>A list of groups.</returns>
    [HttpGet]
    public async Task<ActionResult<List<GroupDto>>> GetGroups()
    {
        var groups = await groupService.GetGroupsAsync();
        var groupsResponse = mapper.Map<List<GroupDto>>(groups);
        return Ok(groupsResponse);
    }
}