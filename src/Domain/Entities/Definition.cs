using Domain.Common;
using Domain.Enums;
using Domain.Events;
using Domain.Helpers;

namespace Domain.Entities;

public sealed class Definition : BaseEntity
{
    public Guid? ParentId { get; private set; }
    public Definition? Parent { get; private set; }
    public DefinitionType Type { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    public static Definition Create(string name, DefinitionType type, string code, Guid? parentId)
    {
        var definition = new Definition
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Type = type,
            Code = code,
            ParentId = parentId,
            CreatedAt = DateTimeHelper.GetTurkeyTime(),
            IsActive = true
        };

        definition.AddDomainEvent(new DefinitionCreatedEvent(definition));

        return definition;
    }

    public void Update(string name, DefinitionType type, string code, Guid? parentId)
    {
        Name = name;
        Type = type;
        Code = code;
        ParentId = parentId;
        ModifiedAt = DateTimeHelper.GetTurkeyTime();

        AddDomainEvent(new DefinitionUpdatedEvent(this));
    }

    public void SetParent(Definition? parent)
    {
        Parent = parent;
        ParentId = parent?.Id ?? Guid.Empty;
    }
}