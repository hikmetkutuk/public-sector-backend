using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Events.Definition;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Definition.Commands.Update;

public sealed class UpdateDefinitionCommandHandler(IApplicationDbContext dbContext, IMediator mediator)
    : IRequestHandler<UpdateDefinitionCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(UpdateDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = await dbContext.Definitions
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (definition == null)
        {
            return ResponseDto<Guid>.Error("Definition not found.");
        }

        definition.Update(request.Name, request.Type, request.Code, request.ParentId);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new DefinitionUpdatedEvent(definition), cancellationToken);

            return ResponseDto<Guid>.Success(definition.Id, "Definition updated successfully.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return ResponseDto<Guid>.Error("The data was updated by another process.");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while updating the definition.", ex);
        }
    }
}