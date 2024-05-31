using AutoFixture;
using File = FileGuardian.Domain.Entities.File;

namespace FileGuardian.Domain.UnitTests;

public class FileTests
{
    private readonly Fixture fixture = new();

    [Test]
    public void CreateFile_Success()
    {
        var file = new File { Name = fixture.Create<string>(), Risk = 32 };
        Assert.That(file.Risk, Is.EqualTo(32));
    }

    [Test]
    public void CreateFile_RiskOverLimit_Exception()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new File { Name = fixture.Create<string>(), Risk = 101 });
    }

    [Test]
    public void CreateFile_RiskUnderLimit_Exception()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new File { Name = fixture.Create<string>(), Risk = -1 });
    }
}