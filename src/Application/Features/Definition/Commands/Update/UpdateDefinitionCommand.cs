using Application.Common.Models.Responses;
using Domain.Enums;

namespace Application.Features.Definition.Commands.Update;

using MediatR;

public sealed record UpdateDefinitionCommand(Guid Id, string Name, DefinitionType Type, string Code, Guid? ParentId)
    : IRequest<ResponseDto<Guid>>;