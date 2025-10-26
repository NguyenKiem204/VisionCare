using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class CertificateMapper
{
    public static Certificate ToDomain(VisionCare.Infrastructure.Models.Certificate model)
    {
        return new Certificate
        {
            Id = model.CertificateId,
            Name = model.Name,
            Created = DateTime.UtcNow,
            LastModified = DateTime.UtcNow
        };
    }

    public static VisionCare.Infrastructure.Models.Certificate ToInfrastructure(Certificate domain)
    {
        return new VisionCare.Infrastructure.Models.Certificate
        {
            CertificateId = domain.Id,
            Name = domain.Name
        };
    }
}
