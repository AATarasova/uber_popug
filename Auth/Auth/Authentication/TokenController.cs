using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Authentication;

[Route("api/token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] Authenticate.Args args)
    {
        return await _mediator.Send(new Authenticate.Command(args));
    }
}