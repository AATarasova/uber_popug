using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Users;

[Route("api/users")]
[Authorize]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ListUserData.Response> ListAll()
    {
        return await mediator.Send(new ListUserData.Query());
    }

    [HttpPost]
    public async Task Add([FromBody] CreateUserData.Args args)
    {
        await mediator.Send(new CreateUserData.Command(args));
    }

    [HttpPost("{userId:int}")]
    public async Task UpdateRole(int userId, [FromBody] UpdateRole.Args args)
    {
        await mediator.Send(new UpdateRole.Command(userId, args));
    }
}