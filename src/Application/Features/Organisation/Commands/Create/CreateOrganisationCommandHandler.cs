using Application.Common.Interfaces;
using Application.Common.Models.Responses;
using Domain.Events.Organisation;
using MediatR;

namespace Application.Features.Organisation.Commands.Create;

public sealed class CreateOrganisationCommandHandler(
    IApplicationDbContext dbContext,
    IRedisCache redisCache,
    IMediator mediator
) : IRequestHandler<CreateOrganisationCommand, ResponseDto<Guid>>
{
    public async Task<ResponseDto<Guid>> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
    {
        var organisation = Domain.Entities.Organisation.Create(
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
            request.ParentId,
            request.ConvertedFromId
        );

        dbContext.Organisations.Add(organisation);

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);

            // 📌 Publish Event
            await mediator.Publish(new OrganisationCreatedEvent(organisation), cancellationToken);

            return ResponseDto<Guid>.Success(organisation.Id, "Organisation created successfully.");
        }
        catch (Exception e)
        {
            throw new ApplicationException("An error occurred while creating the organisation.", e);
        }
    }
}