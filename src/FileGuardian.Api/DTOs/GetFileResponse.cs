namespace FileGuardian.Api.DTOs;

public class GetFileResponse
{
    public required string Name { get; set; }
    public int Risk { get; set; }
    public List<int> Users { get; set; } = [];
    public List<int> Groups { get; set; } = [];
}
