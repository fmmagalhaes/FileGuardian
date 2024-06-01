using AutoMapper;
using FileGuardian.Api.DTOs;
using FileGuardian.Application.Services.Interfaces;
using FileGuardian.Domain.BusinessEntities;
using Microsoft.AspNetCore.Mvc;

namespace FileGuardian.Api.Controllers;

[Route("api/files")]
[ApiController]
public class FileController(IFileService fileService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Creates a file.
    /// </summary>
    /// <param name="fileRequest"></param>
    /// <returns>The id of the new file.</returns>
    [HttpPost]
    public async Task<ActionResult<int>> CreateFile([FromBody] CreateFileRequest fileRequest)
    {
        var file = mapper.Map<File>(fileRequest);
        var fileId = await fileService.CreateFileAsync(file);
        return Ok(fileId);
    }

    /// <summary>
    /// Gets the details of a file.
    /// </summary>
    /// <param name="id">The id of the file.</param>
    /// <returns>The file details, including list of users and groups.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<GetFileResponse>> GetFile(int id)
    {
        var file = await fileService.GetFileAsync(id);
        var fileResponse = mapper.Map<GetFileResponse>(file);
        return Ok(fileResponse);
    }

    /// <summary>
    /// Gets a filtered list of files.
    /// </summary>
    /// <param name="riskOver">The lower limit of risk.</param>
    /// <param name="nameContains">Pattern to be searched in the file name.</param>
    /// <returns>A list of files.</returns>
    [HttpGet]
    public async Task<ActionResult<List<GetFilesResponse>>> GetFiles(int? riskOver, string? nameContains)
    {
        var files = await fileService.GetFilesAsync(riskOver, nameContains);
        var filesResponse = mapper.Map<List<GetFilesResponse>>(files);
        return Ok(filesResponse);
    }

    /// <summary>
    /// Gets a list of most shared files.
    /// </summary>
    /// <remarks>
    /// Users can have access to files directly or through groups.
    /// </remarks>
    /// <param name="limit">The maximum number of files to list.</param>
    /// <returns>A list of files.</returns>
    [HttpGet("top")]
    public async Task<ActionResult<List<SharedFile>>> GetTopSharedFiles(int limit)
    {
        var files = await fileService.GetTopSharedFilesAsync(limit);
        return Ok(files);
    }

    /// <summary>
    /// Shares files with users and groups.
    /// </summary>
    /// <param name="id">The id of the file to share.</param>
    /// <param name="shareFileRequest">The list of userIds and groupIds to share the file with.</param>
    [HttpPost("{id}/share")]
    public async Task<IActionResult> ShareFile(int id, [FromBody] ShareFileRequest shareFileRequest)
    {
        await fileService.ShareWithUsersAsync(id, shareFileRequest.Users, shareFileRequest.Groups);
        return Ok();
    }
}