using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Abstractions;

public interface IFileGroupRepository : IJoinTableRepository<FileGroup>
{
}