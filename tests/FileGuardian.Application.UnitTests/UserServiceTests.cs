using AutoFixture;
using FileGuardian.Application.Abstractions;
using FileGuardian.Application.Exceptions;
using FileGuardian.Application.Services;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace FileGuardian.Application.UnitTests;

public class UserServiceTests
{
    private readonly Fixture fixture = new();
    private readonly IUserRepository userRepository = Substitute.For<IUserRepository>();
    private IUserService userService;

    [SetUp]
    public void Setup()
    {
        userService = new UserService(userRepository);
    }

    [Test]
    public async Task CreateUserAsync_NameNotInUse_Success()
    {
        // Arrange
        var user = fixture.Create<User>();
        userRepository.GetByNameAsync(user.Name).ReturnsNull();

        var createdUserId = fixture.Create<int>();
        userRepository.AddAsync(user).Returns(createdUserId);

        // Act
        var result = await userService.CreateUserAsync(user);

        // Assert
        Assert.That(result, Is.EqualTo(createdUserId));
    }

    [Test]
    public void CreateUserAsync_NameAlreadyInUse_ThrowException()
    {
        // Arrange
        var user = fixture.Create<User>();
        var existingUser = fixture.Create<User>();
        userRepository.GetByNameAsync(user.Name).Returns(existingUser);

        // Act
        Assert.ThrowsAsync<NameAlreadyInUseException>(async () => await userService.CreateUserAsync(user));
    }

    [Test]
    public async Task GetUsersAsync_SuccessAsync()
    {
        // Arrange
        var users = fixture.CreateMany<User>().ToList();
        userRepository.GetUsersAsync().Returns(users);

        // Act
        var result = await userService.GetUsersAsync();

        // Assert
        Assert.That(result, Is.EqualTo(users));
    }
}