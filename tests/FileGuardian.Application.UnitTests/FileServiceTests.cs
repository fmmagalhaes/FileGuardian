using AutoFixture;
using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Exceptions;
using FileGuardian.Application.Services;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.BusinessEntities;
using FileGuardian.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FileGuardian.Application.UnitTests;

public class FileServiceTests
{
    private readonly Fixture fixture = new();
    private readonly IFileRepository fileRepository = Substitute.For<IFileRepository>();
    private readonly IFileUserRepository fileUserRepository = Substitute.For<IFileUserRepository>();
    private readonly IFileGroupRepository fileGroupRepository = Substitute.For<IFileGroupRepository>();
    private readonly ILogger<FileService> logger = Substitute.For<ILogger<FileService>>();
    private IFileService fileService;

    [SetUp]
    public void Setup()
    {
        fileService = new FileService(fileRepository, fileUserRepository, fileGroupRepository, logger);
    }

    [Test]
    public async Task ShareWithUsersAsync_CallsBulkUpsert()
    {
        // Arrange
        var fileId = fixture.Create<int>();
        var userIds = fixture.CreateMany<int>().ToList();
        var groupIds = fixture.CreateMany<int>().ToList();

        // Act
        await fileService.ShareWithUsersAsync(fileId, userIds, groupIds);

        // Assert
        _ = fileUserRepository.Received().BulkUpsertAsync(Arg.Is<IEnumerable<FileUser>>(x => x.Count() == userIds.Count));
        _ = fileGroupRepository.Received().BulkUpsertAsync(Arg.Is<IEnumerable<FileGroup>>(x => x.Count() == groupIds.Count));
    }

    [Test]
    public void GetFileAsync_FailsWhenNotFound()
    {
        // Arrange
        var fileId = fixture.Create<int>();
        fileRepository.GetByIdAsync(fileId).ReturnsNull();

        // Act and Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await fileService.GetFileAsync(fileId));
    }


    [Test]
    public async Task GetTopSharedFilesAsync_Success()
    {
        // Arrange
        var sharedFiles = fixture.CreateMany<SharedFile>().ToList();
        fileRepository.GetTopSharedFilesAsync(Arg.Any<int>()).Returns(sharedFiles);

        // Act
        var result = await fileService.GetTopSharedFilesAsync(8);

        // Assert
        Assert.That(result, Is.EqualTo(sharedFiles));
    }

    [Test]
    public void GetTopSharedFilesAsync_FailsIfOverLimit()
    {
        // Act and Assert
        Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await fileService.GetTopSharedFilesAsync(31));
    }
}