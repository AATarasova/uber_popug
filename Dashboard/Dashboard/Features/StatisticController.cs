using Dashboard.AccessRights;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Features;

[Route("api/statistic")]
[Authorize]
[ApiController]
public class StatisticController(ISender mediator, AccessRightsManager accessRightsManager) : ControllerBase
{
    [HttpGet("tasks")]
    public async Task<IActionResult> GetTasksRating([FromBody] GetTasksRating.Args args)
    {
        if (!CanView(Permissions.TasksRatingView))
        {
            return new ForbidResult();
        }

        await mediator.Send(new GetTasksRating.Query(args));
        return new OkResult();
    }
    
    [HttpGet("employees")]
    public async Task<IActionResult> GetEmployeeStatistic([FromBody] GetEmployeeStatistic.Args args)
    {
        if (!CanView(Permissions.EmployeeStatisticView))
        {
            return new ForbidResult();
        }

        await mediator.Send(new GetEmployeeStatistic.Query(args));
        return new OkResult();
    }
    
    [HttpGet("company")]
    public async Task<IActionResult> GetCompanyInfo([FromBody] GetCompanyInfo.Args args)
    {
        if (!CanView(Permissions.TopManagementBalanceView))
        {
            return new ForbidResult();
        }

        await mediator.Send(new GetCompanyInfo.Query(args));
        return new OkResult();
    }

    private bool CanView(Permissions permission)
    {
        var roleClaim = User.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;
        var canParse = Enum.TryParse(roleClaim, out Role role);
        return !canParse || !accessRightsManager.HasPermission(role, permission);
    }
}