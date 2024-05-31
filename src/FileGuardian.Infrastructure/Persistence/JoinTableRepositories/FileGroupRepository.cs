using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.Entities;

namespace FileGuardian.Infrastructure.Persistence.JoinTables;

public class FileGroupRepository(FileGuardianDbContext dbContext) : AbstractJoinTableRepository<FileGroup>(dbContext), IFileGroupRepository
{
}
