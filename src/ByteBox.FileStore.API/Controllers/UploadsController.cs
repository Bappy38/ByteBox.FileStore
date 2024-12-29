using ByteBox.FileStore.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UploadsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Initiate-Multipart")]
    public async Task<IActionResult> InitiateMultipartUpload([FromBody] InitiateMultipartUploadCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("{FileId}/Part-Presigned")]
    public async Task<IActionResult> GeneratePartPresignedUrl([FromBody] GeneratePartPresignedCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("Complete-Multipart")]
    public async Task<IActionResult> CompleteMultipartUpload([FromBody] CompleteMultipartUploadCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
