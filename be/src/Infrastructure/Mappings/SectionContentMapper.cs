using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class SectionContentMapper
{
    public static SectionContent ToDomain(VisionCare.Infrastructure.Models.Sectioncontent model)
    {
        return new SectionContent
        {
            SectionKey = model.SectionKey,
            Content = model.Content,
            ImageUrl = model.ImageUrl,
            MoreData = model.MoreData,
        };
    }

    public static VisionCare.Infrastructure.Models.Sectioncontent ToInfrastructure(SectionContent domain)
    {
        return new VisionCare.Infrastructure.Models.Sectioncontent
        {
            SectionKey = domain.SectionKey,
            Content = domain.Content,
            ImageUrl = domain.ImageUrl,
            MoreData = domain.MoreData,
        };
    }
}


