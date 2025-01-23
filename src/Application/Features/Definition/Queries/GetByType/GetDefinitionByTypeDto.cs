namespace Application.Features.Definition.Queries.GetByType;

public sealed record GetDefinitionByTypeDto
{
    public Guid Id { get; }
    public string Name { get; }
}