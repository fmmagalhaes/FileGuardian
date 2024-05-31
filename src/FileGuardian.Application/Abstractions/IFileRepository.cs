using FileGuardian.Domain.BusinessEntities;
using FileGuardian.Domain.Specifications;

namespace FileGuardian.Application.Abstractions;

public interface IFileRepository
{
    Task<int> AddAsync(File file);
    Task<File?> GetByIdAsync(int id);
    Task<List<File>> GetFilesAsync(Specification<File> spec);
    Task<List<SharedFile>> GetTopSharedFilesAsync(int limit);
}
