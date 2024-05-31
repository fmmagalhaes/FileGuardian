namespace FileGuardian.Domain.Entities;

public class FileGroup
{
    public int FileId { get; set; }
    public int GroupId { get; set; }

    // Navigation properties
    public File? File { get; set; }
    public Group? Group { get; set; }
}
