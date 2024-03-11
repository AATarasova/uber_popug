using Accounting.AccessRights;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.WorkingDay;

[Route("api/workday/finish")]
[Authorize]
[ApiController]
public class WorkingDayController(ISender mediator, AccessRightsManager accessRightsManager) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Finish()
    {
        var roleClaim = User.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;
        var canParse = Enum.TryParse(roleClaim, out Role role);
        if (!canParse || !accessRightsManager.HasPermission(role, Permissions.FinishWorkingDay))
        {
            return new ForbidResult();
        }

        await mediator.Send(new Finish.Command());
        return new OkResult();
    }
}