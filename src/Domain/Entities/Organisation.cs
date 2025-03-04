using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Organisation : BaseEntity
{
    public string Name { get; private set; }
    public string? Latitude { get; private set; }
    public string? Longitude { get; private set; }
    public Guid? ParentId { get; private set; }
    public string? Address { get; private set; }
    public string? PhoneNumber { get; private set; }
    public Guid? GeographicRegionDefId { get; private set; }
    public Guid? ProcedureRegionDefId { get; private set; }
    public byte? CenterClass { get; private set; }
    public LocalStatus? LocalStatus { get; private set; }
    public int? AtmCount { get; private set; }
    public int? BranchCount { get; private set; }
    public DistributionStatus? DistributionStatus { get; private set; }
    public int? WageTakeMachine { get; private set; }
    public int? WagePayMachine { get; private set; }
    public DateTime? DocumentDate { get; private set; }
    public int? DocumentNumber { get; private set; }
    public string? Note { get; private set; }
    public OrganisationType? OrganisationType { get; private set; }
    public Guid? ConvertedFromId { get; private set; }
    public string? Email { get; private set; }

    public Organisation? Parent { get; private set; }
    public Organisation? ConvertedFrom { get; private set; }
    public Definition? GeographicRegionDef { get; private set; }
    public Definition? ProcedureRegionDef { get; private set; }

    public ICollection<Organisation> Children { get; private set; } = new List<Organisation>();
    public ICollection<OrganisationLog> OrganisationLogs { get; private set; } = new List<OrganisationLog>();

    private Organisation()
    {
    }

    public static Organisation Create(
        string name, string? latitude, string? longitude, string? address, string? phoneNumber,
        byte? centerClass, LocalStatus? localStatus, int? atmCount, int? branchCount,
        DistributionStatus? distributionStatus, int? wageTakeMachine, int? wagePayMachine,
        DateTime? documentDate, int? documentNumber, string? note, OrganisationType? organisationType,
        Guid? geographicRegionDefId, Guid? procedureRegionDefId,
        string? email, Guid? parentId = null, Guid? convertedFromId = null)
    {
        return new Organisation
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Latitude = latitude,
            Longitude = longitude,
            Address = address,
            PhoneNumber = phoneNumber,
            GeographicRegionDefId = geographicRegionDefId,
            ProcedureRegionDefId = procedureRegionDefId,
            CenterClass = centerClass,
            LocalStatus = localStatus,
            AtmCount = atmCount,
            BranchCount = branchCount,
            DistributionStatus = distributionStatus,
            WageTakeMachine = wageTakeMachine,
            WagePayMachine = wagePayMachine,
            DocumentDate = documentDate,
            DocumentNumber = documentNumber,
            Note = note,
            OrganisationType = organisationType,
            Email = email,
            ParentId = parentId,
            ConvertedFromId = convertedFromId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        string name, string? latitude, string? longitude, string? address, string? phoneNumber,
        byte? centerClass, LocalStatus? localStatus, int? atmCount, int? branchCount,
        DistributionStatus? distributionStatus, int? wageTakeMachine, int? wagePayMachine,
        DateTime? documentDate, int? documentNumber, string? note, OrganisationType? organisationType,
        Guid? geographicRegionDefId, Guid? procedureRegionDefId,
        string? email, Guid? parentId = null)
    {
        Name = name;
        Latitude = latitude;
        Longitude = longitude;
        Address = address;
        PhoneNumber = phoneNumber;
        GeographicRegionDefId = geographicRegionDefId;
        ProcedureRegionDefId = procedureRegionDefId;
        CenterClass = centerClass;
        LocalStatus = localStatus;
        AtmCount = atmCount;
        BranchCount = branchCount;
        DistributionStatus = distributionStatus;
        WageTakeMachine = wageTakeMachine;
        WagePayMachine = wagePayMachine;
        DocumentDate = documentDate;
        DocumentNumber = documentNumber;
        Note = note;
        OrganisationType = organisationType;
        Email = email;
        ParentId = parentId;
        ModifiedAt = DateTime.UtcNow;
    }

    public void SetParent(Organisation? parent)
    {
        Parent = parent;
        ParentId = parent?.Id;
    }
}