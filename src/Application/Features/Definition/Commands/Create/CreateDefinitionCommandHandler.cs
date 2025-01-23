using Application.Common.Interfaces;
using MediatR;

namespace Application.Features.Definition.Commands.Create;

public sealed class CreateDefinitionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateDefinitionCommand, Guid>
{
    public async Task<Guid> Handle(CreateDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = Domain.Entities.Definition.Create(request.Name, request.Type, request.Code, request.ParentId);

        dbContext.Definitions.Add(definition);

        await dbContext.SaveChangesAsync(cancellationToken);

        return definition.Id;
    }
}