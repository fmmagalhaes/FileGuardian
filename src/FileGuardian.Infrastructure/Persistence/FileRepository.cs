using FileGuardian.Application.Abstractions;
using FileGuardian.Domain.BusinessEntities;
using FileGuardian.Domain.Specifications;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FileGuardian.Infrastructure.Persistence;

public class FileRepository(FileGuardianDbContext dbContext) : IFileRepository
{
    public async Task<int> AddAsync(File file)
    {
        await dbContext.AddAsync(file);
        await dbContext.SaveChangesAsync();

        return file.Id;
    }

    public async Task<File?> GetByIdAsync(int id)
    {
        return await dbContext.Files
            .AsNoTracking()
            .Include(f => f.FileUsers)
            .Include(f => f.FileGroups)
            .SingleOrDefaultAsync(f => f.Id == id);
    }

    public async Task<List<File>> GetFilesAsync(Specification<File> spec)
    {
        // TODO: Implement pagination
        return await dbContext.Files
            .AsNoTracking()
            .Where(spec.ToExpression())
            .OrderByDescending(f => f.Risk)
            .ToListAsync();
    }

    // Returns a list of files and the users with access to them
    // Users can have access to files directly or through groups
    public async Task<List<SharedFile>> GetTopSharedFilesAsync(int limit)
    {
        var query = @"
            WITH DirectFileUsers AS (
                SELECT f.id AS fileId, fu.userId AS userId, f.name AS fileName, f.risk AS fileRisk
                FROM Files f
                JOIN FileUsers fu ON f.id = fu.fileId
            ),
            GroupFileUsers AS (
                SELECT f.id AS fileId, gu.userId AS userId, f.name AS fileName, f.risk AS fileRisk
                FROM Files f
                JOIN FileGroups fg ON f.id = fg.fileId
                JOIN GroupUsers gu ON fg.groupId = gu.groupId
            ),
            AllFileUsers AS (
                SELECT * FROM DirectFileUsers
                UNION ALL
                SELECT * FROM GroupFileUsers
            )
            SELECT fileId as Id, fileName as Name, fileRisk as Risk, COUNT(DISTINCT userId) AS userCount, json_group_array(DISTINCT u.Name) AS Users
            FROM AllFileUsers afu
            JOIN Users u ON afu.userId = u.id
            GROUP BY fileId, fileName, fileRisk
            ORDER BY userCount DESC
            LIMIT @numberOfFiles;";

        var sqlParameters = new SqliteParameter[]
        {
            new ("@numberOfFiles", limit),
        };
        return await dbContext.Database.SqlQueryRaw<SharedFile>(query, sqlParameters).ToListAsync();
    }

}
