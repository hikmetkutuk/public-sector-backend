using Application.Common.Models.Responses;
using Application.Features.Organisation.Commands.Create;
using Application.Features.Organisation.Queries.GetByParentId;
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

    [HttpGet("{parentId:guid?}")]
    public async Task<IActionResult> GetByParentId(Guid? parentId, CancellationToken cancellationToken)
    {
        var query = new GetOrganisationByParentIdQuery(parentId);
        var result = await mediator.Send(query, cancellationToken);

        if (!result.Any())
        {
            return NotFound(new
            {
                Message = parentId.HasValue
                    ? "No organization found for the specified parent ID."
                    : "No organization with type 0 was found."
            });
        }

        return Ok(result);
    }
}