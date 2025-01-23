using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public sealed class Definition : BaseEntity
{
    public Guid? ParentId { get; private set; }
    public Definition? Parent { get; private set; }
    public DefinitionType Type { get; private set; }
    public string Code { get; private set; }
    public string Name { get; private set; }

    public static Definition Create(string name, DefinitionType type, string code, Definition requestParentId,
        Guid? parentId = null)
    {
        return new Definition
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Type = type,
            Code = code,
            ParentId = parentId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void SetParent(Definition? parent)
    {
        if (parent == null)
        {
            Parent = null;
            ParentId = Guid.Empty;
        }
        else
        {
            Parent = parent;
            ParentId = parent.Id;
        }
    }
}