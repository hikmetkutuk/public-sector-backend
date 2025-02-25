using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Events;
using Domain.Events.Definition;
using MediatR;

namespace Application.Features.Definition.Commands.Delete;

public class DeleteDefinitionCommandHandler(IApplicationDbContext dbContext, IMediator mediator)
    : IRequestHandler<DeleteDefinitionCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(DeleteDefinitionCommand request, CancellationToken cancellationToken)
    {
        var definition = await dbContext.Definitions.FindAsync(new object?[] { request.DefinitionId },
            cancellationToken: cancellationToken);

        try
        {
            // 📌 Delete Definition
            if (definition == null) return ResponseDto<Guid>.Success("Definition deleted successfully.");
            dbContext.Definitions.Remove(definition);
            await dbContext.SaveChangesAsync(cancellationToken);

            // 📌 Triggering domain event
            await mediator.Publish(new DefinitionDeletedEvent(definition), cancellationToken);

            return ResponseDto<Guid>.Success("Definition deleted successfully.");
        }
        catch (Exception ex)
        {
            return ResponseDto<Guid>.Error("An error occurred while deleting the definition. " + ex);
        }
    }
}