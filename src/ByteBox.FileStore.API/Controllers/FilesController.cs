using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IMediator _mediator;

    public FilesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Initiate-Multipart-Upload")]
    public async Task<IActionResult> InitiateMultipartUpload([FromBody] InitiateMultipartUploadCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("{FileId}/Generate-Part-Presigned-Url")]
    public async Task<IActionResult> GeneratePartPresignedUrl([FromBody] GeneratePartPresignedCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("Complete-Multipart-Upload")]
    public async Task<IActionResult> CompleteMultipartUpload([FromBody] CompleteMultipartUploadCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("{fileId:guid}")]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        var query = new GetFileDownloadUrlQuery { FileId = fileId };
        var response = await _mediator.Send(query);
        return Ok(response);
    }

    [HttpDelete("{fileId:guid}")]
    public async Task<IActionResult> DeleteFile(Guid fileId)
    {
        var command = new DeleteFileCommand
        {
            FileId = fileId
        };
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPost("{fileId:guid}/Restore")]
    public async Task<IActionResult> RestoreFile(Guid fileId)
    {
        var command = new RestoreFileCommand
        {
            FileId = fileId
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
