namespace FileGuardian.Domain.Entities;

public class Group
{
    public int Id { get; set; }
    public required string Name { get; set; }

    // Navigation properties
    public List<GroupUser> GroupUsers { get; set; } = [];
}
