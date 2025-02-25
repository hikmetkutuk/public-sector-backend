using Domain.Common;

namespace Domain.Events.Definition;

public sealed class DefinitionUpdatedEvent(Entities.Definition definition) : IDomainEvent
{
    public Entities.Definition Definition { get; } = definition;
}