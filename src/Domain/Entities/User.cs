using Domain.Common;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : BaseEntity
{
    public Email Email { get; set; }
    public FullName FullName { get; set; }
    public PhoneNumber PhoneNumber { get; set; }
}