namespace FileGuardian.Api.DTOs;

public class CreateFileRequest
{
    public required string Name { get; set; }
    public int Risk { get; set; }
}
