using VisionCare.Domain.Entities;

namespace VisionCare.Infrastructure.Mappings;

public static class CommentBlogMapper
{
    public static CommentBlog ToDomain(VisionCare.Infrastructure.Models.Commentblog model)
    {
        return new CommentBlog
        {
            CommentId = model.CommentId,
            BlogId = model.BlogId,
            AuthorId = model.AuthorId,
            ParentCommentId = model.ParentCommentId,
            CommentText = model.CommentText,
            Status = model.Status,
            CreatedAt = model.CreatedAt,
        };
    }

    public static VisionCare.Infrastructure.Models.Commentblog ToInfrastructure(CommentBlog domain)
    {
        return new VisionCare.Infrastructure.Models.Commentblog
        {
            CommentId = domain.CommentId,
            BlogId = domain.BlogId,
            AuthorId = domain.AuthorId,
            ParentCommentId = domain.ParentCommentId,
            CommentText = domain.CommentText,
            Status = domain.Status,
            CreatedAt = domain.CreatedAt,
        };
    }
}

