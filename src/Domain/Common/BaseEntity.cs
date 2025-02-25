namespace Domain.Common;

public abstract class BaseEntity : ICreatedByEntity, IModifiedByEntity
{
    private bool _isActive = true;

    // * used virtual to override
    public virtual Guid Id { get; set; }
    public virtual string? CreatedByUserId { get; set; }
    public virtual DateTimeOffset CreatedAt { get; set; }
    public virtual string? ModifiedByUserId { get; set; }
    public virtual DateTimeOffset? ModifiedAt { get; set; }
    public virtual bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    public virtual bool IsDeleted { get; set; }

    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}