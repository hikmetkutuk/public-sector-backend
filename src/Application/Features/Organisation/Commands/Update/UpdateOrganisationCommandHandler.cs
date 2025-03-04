using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Events.Organisation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Organisation.Commands.Update;

public sealed class UpdateOrganisationCommandHandler(IApplicationDbContext dbContext, IMediator mediator)
    : IRequestHandler<UpdateOrganisationCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(UpdateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisation = await dbContext.Organisations
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken);

        if (organisation == null)
        {
            return ResponseDto<Guid>.Error("Organisation not found.");
        }

        organisation.Update(
            request.Name,
            request.Latitude,
            request.Longitude,
            request.Address,
            request.PhoneNumber,
            request.CenterClass,
            request.LocalStatus,
            request.AtmCount,
            request.BranchCount,
            request.DistributionStatus,
            request.WageTakeMachine,
            request.WagePayMachine,
            request.DocumentDate,
            request.DocumentNumber,
            request.Note,
            request.OrganisationType,
            request.ProcedureRegionDefId,
            request.GeographicRegionDefId,
            request.Email,
            request.ParentId
        );

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            await mediator.Publish(new OrganisationUpdatedEvent(organisation), cancellationToken);

            return ResponseDto<Guid>.Success(organisation.Id, "Organisation updated successfully.");
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return ResponseDto<Guid>.Error("The data was updated by another process.");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("An error occurred while updating the organisation.", ex);
        }
    }
}