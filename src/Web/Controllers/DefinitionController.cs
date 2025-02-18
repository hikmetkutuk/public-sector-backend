using Application.Common.Models.Responses;
using Application.Features.Definition.Commands.Create;
using Application.Features.Definition.Commands.Update;
using Application.Features.Definition.Queries.GetByType;
using Domain.Enums;
using Domain.Extensions;
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

    [HttpGet("{type}")]
    public async Task<IActionResult> GetByType(DefinitionType type, CancellationToken cancellationToken)
    {
        var query = new GetDefinitionByTypeQuery(type);
        var result = await mediator.Send(query, cancellationToken);

        if (!result.Any())
        {
            return NotFound(new { Message = "No definitions found for the given type." });
        }

        return Ok(result);
    }

    [HttpGet("enum")]
    public IActionResult GetEnumDefinitions()
    {
        var enumDefinitions = Enum.GetValues(typeof(DefinitionType))
            .Cast<DefinitionType>()
            .ToDictionary(
                value => value.ToString(),
                value => new
                {
                    Value = (int)value,
                    Description = value.GetDescription()
                }
            );

        return Ok(enumDefinitions);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ResponseDto<Guid>>> UpdateDefinition(Guid id,
        [FromBody] UpdateDefinitionCommand command)
    {
        var updatedCommand = command with { Id = id };

        var response = await mediator.Send(updatedCommand);

        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}