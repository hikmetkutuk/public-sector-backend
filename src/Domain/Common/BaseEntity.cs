namespace Domain.Common;

public abstract class BaseEntity : ICreatedByEntity, IModifiedByEntity
{
    // * used virtual to override
    // ! See: https://furkan-dvlp.medium.com/generating-unique-ids-with-twitter-snowflake-approach-752efa633826
    public virtual long Id { get; set; }
    public virtual string CreatedByUserId { get; set; }
    public virtual DateTimeOffset CreatedAt { get; set; }
    public virtual string? ModifiedByUserId { get; set; }
    public virtual DateTimeOffset? ModifiedAt { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }
}