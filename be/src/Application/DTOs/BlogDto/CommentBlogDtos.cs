using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VisionCare.Application.DTOs.BlogDto;

public class CommentBlogDto
{
    public int CommentId { get; set; }
    public int BlogId { get; set; }
    public int? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public string? AuthorAvatar { get; set; }
    public int? ParentCommentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
    public string? Status { get; set; }

    [JsonConverter(typeof(DateTimeJsonConverter))]
    public DateTime? CreatedAt { get; set; }
    public List<CommentBlogDto> Replies { get; set; } = new();
}

public class DateTimeJsonConverter : System.Text.Json.Serialization.JsonConverter<DateTime?>
{
    public override DateTime? Read(
        ref System.Text.Json.Utf8JsonReader reader,
        Type typeToConvert,
        System.Text.Json.JsonSerializerOptions options
    )
    {
        if (reader.TokenType == System.Text.Json.JsonTokenType.Null)
            return null;

        if (reader.TokenType == System.Text.Json.JsonTokenType.String)
        {
            var dateString = reader.GetString();
            if (string.IsNullOrEmpty(dateString) || !DateTime.TryParse(dateString, out var date))
                return null;
            return date;
        }

        return null;
    }

    public override void Write(
        System.Text.Json.Utf8JsonWriter writer,
        DateTime? value,
        System.Text.Json.JsonSerializerOptions options
    )
    {
        if (value == null)
        {
            writer.WriteNullValue();
            return;
        }

        var localTime = value.Value;
        var offset = TimeZoneInfo.Local.GetUtcOffset(localTime);
        var sign = offset.TotalMinutes >= 0 ? "+" : "-";
        var isoString = localTime.ToString(
            $"yyyy-MM-ddTHH:mm:ss{sign}{Math.Abs(offset.Hours):00}:{Math.Abs(offset.Minutes):00}"
        );
        writer.WriteStringValue(isoString);
    }
}

public class CreateCommentRequest
{
    public int BlogId { get; set; }
    public int? ParentCommentId { get; set; }
    public string CommentText { get; set; } = string.Empty;
}

public class UpdateCommentRequest
{
    public string CommentText { get; set; } = string.Empty;
    public string? Status { get; set; }
}