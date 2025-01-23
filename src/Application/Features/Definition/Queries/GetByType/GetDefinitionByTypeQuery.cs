using MediatR;

namespace Application.Features.Definition.Queries.GetByType;

public record GetDefinitionByTypeQuery(string Type) : IRequest<IEnumerable<GetDefinitionByTypeDto>>;