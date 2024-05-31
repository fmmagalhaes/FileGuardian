namespace FileGuardian.Application.Services;

using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Exceptions;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.BusinessEntities;
using FileGuardian.Domain.Entities;
using FileGuardian.Domain.Specifications;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FileService(
    IFileRepository fileRepository,
    IFileUserRepository fileUserRepository,
    IFileGroupRepository fileGroupRepository,
    ILogger<FileService> logger) : IFileService
{
    public async Task<int> CreateFileAsync(File file)
    {
        return await fileRepository.AddAsync(file);
    }

    public async Task<File> GetFileAsync(int id)
    {
        var file = await fileRepository.GetByIdAsync(id);
        return file ?? throw new NotFoundException($"File with id {id} was not found");
    }

    public async Task<List<File>> GetFilesAsync(int? riskOver, string? nameContains)
    {
        var spec = Specification<File>.All;

        if (riskOver.HasValue)
        {
            spec = spec.And(new RiskSpecification(riskOver.Value));
        }

        if (!string.IsNullOrEmpty(nameContains))
        {
            spec = spec.And(new NameContainsSpecification(nameContains));
        }

        return await fileRepository.GetFilesAsync(spec);
    }

    public async Task<List<SharedFile>> GetTopSharedFilesAsync(int limit)
    {
        if (limit < 1 || limit > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 10");
        }
        return await fileRepository.GetTopSharedFilesAsync(limit);
    }

    public async Task ShareWithUsersAsync(int fileId, List<int> userIds, List<int> groupIds)
    {
        var fileUsers = userIds.Select(userId => new FileUser { FileId = fileId, UserId = userId });
        await fileUserRepository.BulkUpsertAsync(fileUsers);
        logger.LogInformation("Finished sharing file {FileId} with {UserCount} users", fileId, userIds.Count);

        var fileGroups = groupIds.Select(groupId => new FileGroup { FileId = fileId, GroupId = groupId });
        await fileGroupRepository.BulkUpsertAsync(fileGroups);
        logger.LogInformation("Finished sharing file {FileId} with {GroupCount} groups", fileId, groupIds.Count);
    }
}
