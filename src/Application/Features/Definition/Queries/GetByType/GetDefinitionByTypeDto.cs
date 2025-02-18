using Domain.Enums;
using Domain.Extensions;

namespace Application.Features.Definition.Queries.GetByType;

public sealed record GetDefinitionByTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public int Type { get; set; }
    public Guid? ParentId { get; set; }
    public string ParentName { get; set; }

    public string TypeText => ((DefinitionType)Type).GetDescription();
}