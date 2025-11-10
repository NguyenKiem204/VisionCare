using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class BlogMapper
{
    public static Blog ToDomain(VisionCare.Infrastructure.Models.Blog model)
    {
        return new Blog
        {
            BlogId = model.BlogId,
            Title = model.Title,
            Slug = model.Slug,
            Content = model.Content,
            Excerpt = model.Excerpt,
            FeaturedImage = model.FeaturedImage,
            AuthorId = model.AuthorId,
            Status = model.Status,
            PublishedAt = model.PublishedAt,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            ViewCount = model.ViewCount ?? 0,
        };
    }

    public static VisionCare.Infrastructure.Models.Blog ToInfrastructure(Blog domain)
    {
        return new VisionCare.Infrastructure.Models.Blog
        {
            BlogId = domain.BlogId,
            Title = domain.Title,
            Slug = domain.Slug,
            Content = domain.Content,
            Excerpt = domain.Excerpt,
            FeaturedImage = domain.FeaturedImage,
            AuthorId = domain.AuthorId,
            Status = domain.Status,
            PublishedAt = domain.PublishedAt,
            CreatedAt = domain.CreatedAt,
            UpdatedAt = domain.UpdatedAt,
            ViewCount = domain.ViewCount,
        };
    }
}

