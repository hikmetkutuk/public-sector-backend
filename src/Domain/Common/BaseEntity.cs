namespace Domain.Common;

public abstract class BaseEntity : ICreatedByEntity, IModifiedByEntity
{
    // * used virtual to override
    public virtual Guid Id { get; set; }
    public virtual string CreatedByUserId { get; set; }
    public virtual DateTimeOffset CreatedAt { get; set; }
    public virtual string? ModifiedByUserId { get; set; }
    public virtual DateTimeOffset? ModifiedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}