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
    [HttpPost]
    public async Task<ActionResult<int>> CreateFile([FromBody] CreateFileRequest fileDto)
    {
        var file = mapper.Map<File>(fileDto);
        var fileId = await fileService.CreateFileAsync(file);
        return Ok(fileId);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetFileResponse>> GetFile(int id)
    {
        var file = await fileService.GetFileAsync(id);
        var fileResponse = mapper.Map<GetFileResponse>(file);
        return Ok(fileResponse);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetFilesResponse>>> GetFiles(int? riskOver, string? nameContains)
    {
        var files = await fileService.GetFilesAsync(riskOver, nameContains);
        var filesResponse = mapper.Map<List<GetFilesResponse>>(files);
        return Ok(filesResponse);
    }

    [HttpGet("top")]
    public async Task<ActionResult<List<SharedFile>>> GetTopSharedFiles(int limit)
    {
        var files = await fileService.GetTopSharedFilesAsync(limit);
        return Ok(files);
    }

    [HttpPost("{id}/share")]
    public async Task<IActionResult> ShareFileWithUsers(int id, [FromBody] ShareFileRequest shareFileRequest)
    {
        await fileService.ShareWithUsersAsync(id, shareFileRequest.Users, shareFileRequest.Groups);
        return Ok();
    }
}