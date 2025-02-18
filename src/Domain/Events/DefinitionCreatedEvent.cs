using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public sealed class DefinitionCreatedEvent(Definition definition) : IDomainEvent
{
    public Definition Definition { get; } = definition ?? throw new ArgumentNullException(nameof(definition));
}