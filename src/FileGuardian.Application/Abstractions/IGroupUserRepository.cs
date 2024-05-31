using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Abstractions;

public interface IGroupUserRepository : IJoinTableRepository<GroupUser>
{
}