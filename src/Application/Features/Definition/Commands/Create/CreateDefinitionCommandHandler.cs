using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using MediatR;

namespace Application.Features.Definition.Commands.Create;

public sealed class CreateDefinitionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateDefinitionCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(CreateDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = Domain.Entities.Definition.Create(request.Name, request.Type, request.Code, request.ParentId);

        dbContext.Definitions.Add(definition);

        await dbContext.SaveChangesAsync(cancellationToken);

        return ResponseDto<Guid>.Success(definition.Id, "Definition created successfully.");
    }
}