using Application.Common.Models.Responses;
using Domain.Enums;
using MediatR;

namespace Application.Features.Organisation.Commands.Create;

public sealed record CreateOrganisationCommand(
    string Name,
    OrganisationType OrganisationType,
    Guid? ParentId,
    string? Latitude,
    string? Longitude,
    string? PhoneNumber,
    string? Address,
    string? Email,
    byte? CenterClass,
    LocalStatus? LocalStatus,
    int? AtmCount,
    int? BranchCount,
    int? WageTakeMachine,
    int? WagePayMachine,
    DistributionStatus? DistributionStatus,
    DateTime? DocumentDate,
    int? DocumentNumber,
    string? Note,
    Guid? ConvertedFromId,
    Guid? GeographicRegionDefId,
    Guid? ProcedureRegionDefId
) : IRequest<ResponseDto<Guid>>;