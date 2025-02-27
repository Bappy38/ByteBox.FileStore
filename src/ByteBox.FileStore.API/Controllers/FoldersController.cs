using ByteBox.FileStore.Application.Commands;
using ByteBox.FileStore.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FoldersController : ControllerBase
{
    private readonly IMediator _mediator;

    public FoldersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] CreateFolderCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }

    [HttpGet("{folderId:guid}")]
    public async Task<IActionResult> GetFolder(Guid folderId)
    {
        var query = new GetFolderQuery
        {
            FolderId = folderId
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{folderId:guid}/Breadcrumbs")]
    public async Task<IActionResult> GetBreadcrumbs(Guid folderId)
    {
        var query = new GetBreadcrumbsQuery
        {
            FolderId = folderId
        };
        var breadcrumbs = await _mediator.Send(query);
        return Ok(breadcrumbs);
    }

    [HttpPatch("{folderId:guid}")]
    public async Task<IActionResult> RenameFolder(Guid folderId, [FromBody] RenameFolderCommand command)
    {
        if (folderId != command.FolderId)
        {
            return BadRequest("Folder ID in path doesn't match with command");
        }
        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{folderId:guid}")]
    public async Task<IActionResult> DeleteFolder(Guid folderId)
    {
        var command = new DeleteFolderCommand
        {
            FolderId = folderId
        };
        await _mediator.Send(command);
        return NoContent();
    }
}
