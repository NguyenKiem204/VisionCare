using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class BannerMapper
{
    public static Banner ToDomain(VisionCare.Infrastructure.Models.Banner model)
    {
        return new Banner
        {
            BannerId = model.BannerId,
            Title = model.Title,
            Description = model.Description,
            ImageUrl = model.ImageUrl,
            LinkUrl = model.LinkUrl,
            DisplayOrder = model.DisplayOrder,
            Status = model.Status,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            // Place not in DB schema yet; default used
        };
    }

    public static VisionCare.Infrastructure.Models.Banner ToInfrastructure(Banner domain)
    {
        return new VisionCare.Infrastructure.Models.Banner
        {
            BannerId = domain.BannerId,
            Title = domain.Title,
            Description = domain.Description,
            ImageUrl = domain.ImageUrl,
            LinkUrl = domain.LinkUrl,
            DisplayOrder = domain.DisplayOrder,
            Status = domain.Status,
            StartDate = domain.StartDate,
            EndDate = domain.EndDate,
        };
    }
}


