using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileGuardian.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] User user)
    {
        var userId = await userService.CreateUserAsync(user);
        return Ok(userId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }
}