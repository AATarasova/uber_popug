using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TaskTracker.Employees;

[Route("api/employees")]
// [Authorize]
[ApiController]
public class EmployeeController(ISender mediator) : ControllerBase
{
    [HttpPost]
    public async Task Create([FromBody] Create.Args args)
    {
        await mediator.Send(new Create.Command(args));
    }
}