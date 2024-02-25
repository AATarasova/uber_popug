using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Users;

[Route("api/users")]
[Authorize]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ListUserData.Response> ListAll()
    {
        return await _mediator.Send(new ListUserData.Query());
    }

    [HttpPost]
    public async Task Add([FromBody] CreateUserData.Args args)
    {
        await _mediator.Send(new CreateUserData.Command(args));
    }
}