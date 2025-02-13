using Domain.Enums;

namespace Application.Features.Definition.Queries.GetByType;

public sealed record GetDefinitionByTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Type { get; init; }
    public Guid? ParentId { get; init; }
    public string ParentName { get; init; }

    public string TypeText => Enum.GetName(typeof(DefinitionType), Type) ?? "Unknown";
}