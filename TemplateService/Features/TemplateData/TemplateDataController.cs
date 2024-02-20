using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TemplateService.Features.TemplateData;

[Route("api/template")]
[ApiController]
public class TemplateDataController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateDataController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ListTemplateData.Response> ListAll()
    {
        return await _mediator.Send(new ListTemplateData.Query());
    }

    [HttpGet("{id:int}")]
    public async Task<GetTemplateData.Response> Get(int id)
    {
        return await _mediator.Send(new GetTemplateData.Query(id));
    }

    [HttpPost]
    public async Task Add([FromBody] CreateTemplateData.Command command)
    {
        await _mediator.Send(command);
    }
}