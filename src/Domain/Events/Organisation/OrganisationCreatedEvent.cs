using Domain.Common;

namespace Domain.Events.Organisation;

public sealed class OrganisationCreatedEvent(Entities.Organisation organisation) : IDomainEvent
{
    public Entities.Organisation Organisation { get; } = organisation ?? throw new ArgumentNullException(nameof(organisation));
}