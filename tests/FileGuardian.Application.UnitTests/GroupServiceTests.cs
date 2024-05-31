using AutoFixture;
using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Services;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace FileGuardian.Application.UnitTests;

public class GroupServiceTests
{
    private readonly Fixture fixture = new();
    private readonly IGroupRepository groupRepository = Substitute.For<IGroupRepository>();
    private readonly IGroupUserRepository groupUserRepository = Substitute.For<IGroupUserRepository>();
    private readonly ILogger<GroupService> logger = Substitute.For<ILogger<GroupService>>();
    private IGroupService groupService;

    [SetUp]
    public void Setup()
    {
        groupService = new GroupService(groupRepository, groupUserRepository, logger);
    }

    [Test]
    public async Task AddUsersToGroupAsync_CallsBulkUpsert()
    {
        // Arrange
        var groupId = fixture.Create<int>();
        var userIds = fixture.CreateMany<int>().ToList();

        // Act
        await groupService.AddUsersToGroupAsync(groupId, userIds);

        // Assert
        _ = groupUserRepository.Received().BulkUpsertAsync(Arg.Is<IEnumerable<GroupUser>>(x => x.Count() == userIds.Count));
    }
}