using Application.Common.Models.Responses;
using MediatR;

namespace Application.Features.Definition.Commands.Delete;

public class DeleteDefinitionCommand(Guid definitionId) : IRequest<ResponseDto<Guid>>
{
    public Guid DefinitionId { get; } = definitionId;
}