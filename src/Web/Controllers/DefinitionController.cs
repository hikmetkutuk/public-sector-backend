using Application.Common.Models.Responses;
using Application.Features.Definition.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/definition")]
public class DefinitionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseDto<Guid>>> CreateDefinition([FromBody] CreateDefinitionCommand command)
    {
        var response = await mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}