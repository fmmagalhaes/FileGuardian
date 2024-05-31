namespace FileGuardian.Domain.BusinessEntities;

public class SharedFile
{
    public required string Name { get; set; }
    public int Risk { get; set; }
    public List<string> Users { get; set; } = [];
}
