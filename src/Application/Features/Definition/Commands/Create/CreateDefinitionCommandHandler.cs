using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Events;
using MediatR;

namespace Application.Features.Definition.Commands.Create;

public sealed class CreateDefinitionCommandHandler(
    IApplicationDbContext dbContext,
    IRedisCache redisCache,
    IMediator mediator)
    : IRequestHandler<CreateDefinitionCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(CreateDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = Domain.Entities.Definition.Create(request.Name, request.Type, request.Code, request.ParentId);

        dbContext.Definitions.Add(definition);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            // 📌 Publish Event
            await mediator.Publish(new DefinitionCreatedEvent(definition), cancellationToken);

            return ResponseDto<Guid>.Success(definition.Id, "Definition created successfully.");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while creating the definition.", ex);
        }
    }
}