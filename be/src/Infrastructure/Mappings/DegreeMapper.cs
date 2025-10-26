using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class DegreeMapper
{
    public static Degree ToDomain(VisionCare.Infrastructure.Models.Degree model)
    {
        return new Degree
        {
            Id = model.DegreeId,
            Name = model.Name,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static VisionCare.Infrastructure.Models.Degree ToInfrastructure(Degree domain)
    {
        return new VisionCare.Infrastructure.Models.Degree
        {
            DegreeId = domain.Id,
            Name = domain.Name
        };
    }
}
