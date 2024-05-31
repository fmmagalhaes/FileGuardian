namespace FileGuardian.Domain.Entities;

public class FileUser
{
    public int FileId { get; set; }
    public int UserId { get; set; }

    // Navigation properties
    public File? File { get; set; }
    public User? User { get; set; }
}
