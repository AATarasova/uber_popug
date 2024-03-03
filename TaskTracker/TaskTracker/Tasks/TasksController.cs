using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Tasks;

[Route("api/tasks")]
[Authorize]
[ApiController]
public class TasksController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ListOpen.Response> ListAll()
    {
        return await mediator.Send(new ListOpen.Query());
    }

    [HttpPost]
    public async Task Create([FromBody] Create.Args args)
    {
        await mediator.Send(new Create.Command(args));
    }

    [HttpPost("reassign")]
    public async Task Reassign()
    {
        await mediator.Send(new Reassign.Command());
    }
    [HttpPost("{taskId:int}/close")]
    public async Task Close(int taskId)
    {
        await mediator.Send(new Close.Command(taskId));
    }
}