using Domain.Common;

namespace Domain.Events.Definition;

public sealed class DefinitionCreatedEvent(Entities.Definition definition) : IDomainEvent
{
    public Entities.Definition Definition { get; } = definition ?? throw new ArgumentNullException(nameof(definition));
}