namespace FileGuardian.Domain.Entities;

public class GroupUser
{
    public int GroupId { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public Group? Group { get; set; }
    public User? User { get; set; }
}
