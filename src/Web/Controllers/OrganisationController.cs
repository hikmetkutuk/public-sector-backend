using Application.Common.Models.Responses;
using Application.Features.Organisation.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/organisation")]
public class OrganisationController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ResponseDto<Guid>>> CreateOrganisation([FromBody] CreateOrganisationCommand command)
    {
        var response = await mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}