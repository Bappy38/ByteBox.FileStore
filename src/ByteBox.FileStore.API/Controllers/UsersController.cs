using ByteBox.FileStore.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ByteBox.FileStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Created(string.Empty, result);
    }
}
