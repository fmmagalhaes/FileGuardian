using AutoFixture;
using AutoMapper;
using FileGuardian.Api.Controllers;
using FileGuardian.Api.DTOs;
using FileGuardian.Api.Mapping;
using FileGuardian.Application.Services.Interfaces;
using NSubstitute;
using File = FileGuardian.Domain.Entities.File;

namespace FileGuardian.Api.UnitTests;

public class FileControllerTests
{
    private readonly Fixture fixture = new();
    private readonly IFileService fileService = Substitute.For<IFileService>();
    private readonly IMapper mapper;
    private FileController fileController;

    public FileControllerTests()
    {
        var mockMapper = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        mapper = mockMapper.CreateMapper();
    }

    [SetUp]
    public void Setup()
    {
        fileController = new FileController(fileService, mapper);
    }

    [Test]
    public async Task CreateFile_SuccessAsync()
    {
        // Arrange
        var request = fixture
            .Build<CreateFileRequest>()
            .With(x => x.Risk, 30)
            .Create();

        // Act
        await fileController.CreateFile(request);

        // Assert
        _ = fileService.Received().CreateFileAsync(Arg.Is<File>(f => f.Name == request.Name && f.Risk == request.Risk));
    }
}