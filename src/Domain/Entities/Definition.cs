using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class Definition : BaseEntity
{
    public long? ParentId { get; set; }
    public Definition? Parent { get; set; }
    public DefinitionType Type { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}