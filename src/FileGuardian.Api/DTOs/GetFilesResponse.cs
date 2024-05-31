namespace FileGuardian.Api.DTOs;

public class GetFilesResponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Risk { get; set; }
}
