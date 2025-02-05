using Domain.Enums;
using MediatR;

namespace Application.Features.Definition.Queries.GetByType;

public record GetDefinitionByTypeQuery(DefinitionType Type) : IRequest<IEnumerable<GetDefinitionByTypeDto>>;