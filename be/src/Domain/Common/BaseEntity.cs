using System.ComponentModel.DataAnnotations;

namespace VisionCare.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }

    public DateTime Created { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }

    public List<BaseEvent> DomainEvents { get; set; } = new();
}

public abstract class BaseEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
