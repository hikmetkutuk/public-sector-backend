using Domain.Common;
using Domain.Entities;

namespace Domain.Events;

public sealed class DefinitionDeletedEvent(Definition definition) : IDomainEvent
{
    public Definition Definition { get; } = definition ?? throw new ArgumentNullException(nameof(definition));
}