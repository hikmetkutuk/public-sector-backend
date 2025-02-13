using System.ComponentModel;
using Application.Common.Models.Responses;
using Application.Features.Definition.Commands.Create;
using Application.Features.Definition.Queries.GetByType;
using Domain.Enums;
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
                    Description = GetEnumDescription(value)
                }
            );

        return Ok(enumDefinitions);
    }

    private string GetEnumDescription(DefinitionType value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute?.Description ?? value.ToString();
    }
}