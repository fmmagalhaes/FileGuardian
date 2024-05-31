namespace FileGuardian.Application.Services.Interfaces;

using FileGuardian.Domain.BusinessEntities;

public interface IFileService
{
    Task<int> CreateFileAsync(File file);
    Task<File> GetFileAsync(int id);
    Task<List<File>> GetFilesAsync(int? riskOver, string? nameContains);
    Task<List<SharedFile>> GetTopSharedFilesAsync(int limit);
    Task ShareWithUsersAsync(int fileId, List<int> userIds, List<int> groupIds);
}
