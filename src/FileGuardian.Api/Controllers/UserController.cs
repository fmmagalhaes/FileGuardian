using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileGuardian.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    /// <summary>
    /// Creates a user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns>The id of the new user.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateUser([FromBody] User user)
    {
        var userId = await userService.CreateUserAsync(user);
        return Ok(userId);
    }

    /// <summary>
    /// Gets the details of a user.
    /// </summary>
    /// <param name="id">The id of the user.</param>
    /// <returns>The user details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await userService.GetUserAsync(id);
        return Ok(user);
    }

    /// <summary>
    /// Gets a list of all users.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsers()
    {
        var users = await userService.GetUsersAsync();
        return Ok(users);
    }
}