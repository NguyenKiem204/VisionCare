using VisionCare.Domain.Common;

namespace VisionCare.Domain.Entities;

public class Specialization : BaseEntity
{
    public string? SpecializationName { get; set; }
    public string? SpecializationStatus { get; set; }
}
