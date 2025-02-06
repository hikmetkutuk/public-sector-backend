namespace Application.Features.Definition.Queries.GetByType;

public sealed record GetDefinitionByTypeDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}