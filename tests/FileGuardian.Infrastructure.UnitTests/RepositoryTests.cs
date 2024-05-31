using FileGuardian.Domain.Entities;
using FileGuardian.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using File = FileGuardian.Domain.Entities.File;

namespace FileGuardian.Infrastructure.UnitTests;

// based on https://github.com/dotnet/EntityFramework.Docs/blob/main/samples/core/Testing/TestingWithoutTheDatabase/SqliteInMemoryBloggingControllerTest.cs
public class RepositoryTests
{
    private readonly DbContextOptions<FileGuardianDbContext> contextOptions;
    private readonly SqliteConnection connection;

    public RepositoryTests()
    {
        // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
        // at the end of the test (see Dispose below).
        connection = new SqliteConnection("Filename=:memory:");
        connection.Open();

        // These options will be used by the context instances in this test suite, including the connection opened above.
        contextOptions = new DbContextOptionsBuilder<FileGuardianDbContext>()
            .UseSqlite(connection)
            .Options;

        // Create the schema and seed some data
        using var context = new FileGuardianDbContext(contextOptions);

        if (context.Database.EnsureCreated())
        {
            using var viewCommand = context.Database.GetDbConnection().CreateCommand();
            viewCommand.CommandText = @"
                CREATE VIEW AllResources AS
                SELECT Name
                FROM Files;";
            viewCommand.ExecuteNonQuery();
        }
        context.AddRange(
            new File { Id = 1, Name = "file1.txt", Risk = 2 },
            new File { Id = 2, Name = "file2.txt", Risk = 20 });
        context.AddRange(
            new User { Id = 1, Name = "User1" },
            new User { Id = 2, Name = "User2" },
            new User { Id = 3, Name = "User3" });
        context.AddRange(
            new Group { Id = 1, Name = "Group1" },
            new Group { Id = 2, Name = "Group2" });
        context.AddRange(
            new GroupUser { GroupId = 1, UserId = 1 },
            new GroupUser { GroupId = 2, UserId = 2 });

        // File 1 is shared with User3
        context.AddRange(
            new FileUser { FileId = 1, UserId = 3 });

        // File 2 is shared with User1 and Group2 (User2)
        context.AddRange(
            new FileUser { FileId = 2, UserId = 1 });
        context.AddRange(
            new FileGroup { FileId = 2, GroupId = 2 });

        context.SaveChanges();

        return;
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        Dispose();
    }

    [Test]
    public async Task GetFileAsync()
    {
        // Arrange
        var context = CreateContext();
        var repo = new FileRepository(context);

        // Act
        var file = await repo.GetByIdAsync(1);

        // Assert
        Assert.That(file, Is.Not.Null);
        Assert.That(file.Name, Is.EqualTo("file1.txt"));
    }

    [Test]
    public async Task GetUserAsync()
    {
        // Arrange
        var context = CreateContext();
        var repo = new UserRepository(context);

        // Act
        var user = await repo.GetByIdAsync(1);

        // Assert
        Assert.That(user, Is.Not.Null);
        Assert.That(user.Name, Is.EqualTo("User1"));
    }

    [Test]
    public async Task GetGroupAsync()
    {
        // Arrange
        var context = CreateContext();
        var repo = new GroupRepository(context);

        // Act
        var group = await repo.GetByIdAsync(1);

        // Assert
        Assert.That(group, Is.Not.Null);
        Assert.That(group.Name, Is.EqualTo("Group1"));
        var groupUsers = group.GroupUsers;
        Assert.That(groupUsers, Has.Count.EqualTo(1));
        Assert.That(groupUsers[0].User!.Name, Is.EqualTo("User1"));
    }

    [Test]
    public async Task GetTopSharedFiles()
    {
        // Arrange
        var context = CreateContext();
        var repo = new FileRepository(context);

        // Act
        var sharedFiles = await repo.GetTopSharedFilesAsync(10);

        // Assert
        Assert.That(sharedFiles, Has.Count.EqualTo(2));
        Assert.That(sharedFiles[0].Name, Is.EqualTo("file2.txt"));
        Assert.That(sharedFiles[0].Risk, Is.EqualTo(20));
        Assert.That(sharedFiles[0].Users, Has.Count.EqualTo(2));
        Assert.That(sharedFiles[0].Users, Does.Contain("User1"));
        Assert.That(sharedFiles[0].Users, Does.Contain("User2"));

        Assert.That(sharedFiles[1].Name, Is.EqualTo("file1.txt"));
        Assert.That(sharedFiles[1].Risk, Is.EqualTo(2));
        Assert.That(sharedFiles[1].Users, Has.Count.EqualTo(1));
        Assert.That(sharedFiles[1].Users, Does.Contain("User3"));
    }

    FileGuardianDbContext CreateContext() => new(contextOptions);

    private void Dispose() => connection.Dispose();
}