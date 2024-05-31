namespace FileGuardian.Api.DTOs;

public class ShareFileRequest
{
    public List<int> Users { get; set; } = [];
    public List<int> Groups { get; set; } = [];
}
