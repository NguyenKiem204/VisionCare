using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class SpecializationMapper
{
    public static Specialization ToDomain(VisionCare.Infrastructure.Models.Specialization model)
    {
        return new Specialization
        {
            Id = model.SpecializationId,
            SpecializationName = model.Name ?? string.Empty,
            SpecializationStatus = model.Status ?? "Active",
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
        };
    }

    public static VisionCare.Infrastructure.Models.Specialization ToInfrastructure(
        Specialization domain
    )
    {
        return new VisionCare.Infrastructure.Models.Specialization
        {
            SpecializationId = domain.Id,
            Name = domain.SpecializationName ?? string.Empty,
            Status = domain.SpecializationStatus!
        };
    }
}
