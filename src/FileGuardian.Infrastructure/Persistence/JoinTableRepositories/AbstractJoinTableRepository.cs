using FileGuardian.Application.Abstractions;

namespace FileGuardian.Infrastructure.Persistence.JoinTables;

public class AbstractJoinTableRepository<T>(FileGuardianDbContext dbContext) : IJoinTableRepository<T> where T : class
{
    public async Task BulkUpsertAsync(IEnumerable<T> linkRecords)
    {
        await dbContext.BulkMergeAsync(linkRecords);
        await dbContext.SaveChangesAsync();
    }
}
