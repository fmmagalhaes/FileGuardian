using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.Entities;

namespace FileGuardian.Infrastructure.Persistence.JoinTables;

public class FileUserRepository(FileGuardianDbContext dbContext) : AbstractJoinTableRepository<FileUser>(dbContext), IFileUserRepository
{
}
