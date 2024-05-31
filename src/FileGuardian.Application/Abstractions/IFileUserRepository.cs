using FileGuardian.Domain.Entities;

namespace FileGuardian.Application.Abstractions;

public interface IFileUserRepository : IJoinTableRepository<FileUser>
{
}