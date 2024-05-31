namespace FileGuardian.Application.Abstractions;

public interface IJoinTableRepository<T>
{
    Task BulkUpsertAsync(IEnumerable<T> linkRecords);
}