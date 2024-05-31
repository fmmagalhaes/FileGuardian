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
    [HttpPost]
    public async Task<ActionResult<int>> CreateGroup([FromBody] GroupDto groupDto)
    {
        var group = mapper.Map<Group>(groupDto);
        var groupId = await groupService.CreateGroupAsync(group);
        return Ok(groupId);
    }

    [HttpPost("{id}/users")]
    public async Task<ActionResult<int>> AddUsersToGroup(int id, [FromBody] List<int> userIds)
    {
        await groupService.AddUsersToGroupAsync(id, userIds);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetGroupResponse>> GetGroup(int id)
    {
        var group = await groupService.GetGroupAsync(id);
        var groupResponse = mapper.Map<GetGroupResponse>(group);
        return Ok(groupResponse);
    }

    [HttpGet]
    public async Task<ActionResult<List<GroupDto>>> GetGroups()
    {
        var groups = await groupService.GetGroupsAsync();
        var groupsResponse = mapper.Map<List<GroupDto>>(groups);
        return Ok(groupsResponse);
    }
}