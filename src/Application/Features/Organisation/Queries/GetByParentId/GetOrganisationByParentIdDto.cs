using Domain.Enums;
using Domain.Extensions;

namespace Application.Features.Organisation.Queries.GetByParentId;

public record GetOrganisationByParentIdDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public OrganisationType OrganisationType { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }
    public DateTime? DocumentDate { get; set; }
    public int? DocumentNumber { get; set; }
    public int? AtmCount { get; set; }
    public int? BranchCount { get; set; }
    public int? WageTakeMachine { get; set; }
    public int? WagePayMachine { get; set; }
    public byte? CenterClass { get; set; }
    public int? LocalStatus { get; set; }
    public int? DistributionStatus { get; set; }
    public Guid? GeographicRegionDefId { get; set; }
    public Guid? ProcedureRegionDefId { get; set; }
    public string? GeographicRegionDefName { get; set; }
    public string? ProcedureRegionDefName { get; set; }
    public List<GetOrganisationByParentIdDto> Children { get; set; } = new List<GetOrganisationByParentIdDto>();
    public string TypeText => (OrganisationType).GetDescription();
}