using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class OrganisationLog : BaseEntity
{
    public Guid OrganisationId { get; private set; }
    public OrganisationLogType Type { get; private set; }
    public DateTime DocumentDate { get; private set; }
    public string DocumentNumber { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? Note { get; private set; }

    public Organisation Organisation { get; private set; } = null!;

    private OrganisationLog()
    {
    }

    public static OrganisationLog Create(Guid organisationId, OrganisationLogType type, DateTime documentDate,
        string documentNumber, string description, string? note = null)
    {
        return new OrganisationLog
        {
            Id = Guid.CreateVersion7(),
            OrganisationId = organisationId,
            Type = type,
            DocumentDate = documentDate,
            DocumentNumber = documentNumber,
            Description = description,
            Note = note,
            CreatedAt = DateTime.UtcNow
        };
    }
}