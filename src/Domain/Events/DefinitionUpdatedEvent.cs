using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public sealed class DefinitionUpdatedEvent(Definition definition) : IDomainEvent
{
    public Definition Definition { get; } = definition;
}