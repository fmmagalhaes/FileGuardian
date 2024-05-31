using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.Entities;

namespace FileGuardian.Infrastructure.Persistence.JoinTables;

public class GroupUserRepository(FileGuardianDbContext dbContext) : AbstractJoinTableRepository<GroupUser>(dbContext), IGroupUserRepository
{
}
