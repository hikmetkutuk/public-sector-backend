using Domain.Common;

namespace Domain.Events.Organisation;

public sealed class OrganisationUpdatedEvent(Entities.Organisation organisation) : IDomainEvent
{
    public Entities.Organisation Organisation { get; } = organisation;
}