using Domain.Enums;
using MediatR;

namespace Application.Features.Definition.Commands.Create;

public sealed record CreateDefinitionCommand(string Name, DefinitionType Type, string Code, Domain.Entities.Definition ParentId) : IRequest<Guid>;