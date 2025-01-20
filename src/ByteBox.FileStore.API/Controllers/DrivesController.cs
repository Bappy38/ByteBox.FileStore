using ByteBox.FileStore.Application.Queries;
using ByteBox.FileStore.Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DrivesController : ControllerBase
{
    private readonly IMediator _mediator;

    public DrivesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("My-Drive")]
    public async Task<IActionResult> GetMyDrive()
    {
        var query = new GetFolderQuery
        {
            // TODO:: Replace by real user id
            FolderId = Default.User.UserId
        };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Shared-Drive")]
    public async Task<IActionResult> GetSharedDrive()
    {
        return Ok();
    }
}
