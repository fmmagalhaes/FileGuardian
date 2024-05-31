using FileGuardian.Domain.Entities;

namespace FileGuardian.Api.DTOs;

public class GetGroupResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public List<User> Users { get; set; } = [];
}
