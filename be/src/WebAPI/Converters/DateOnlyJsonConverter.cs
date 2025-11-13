using System.Text.Json;
using System.Text.Json.Serialization;

namespace VisionCare.WebAPI.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    private const string Format = "yyyy-MM-dd";

    public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }

            if (DateOnly.TryParseExact(value, Format, out var date))
                return date;

            // Try parsing with other common formats
            if (DateOnly.TryParse(value, out date))
                return date;

            throw new JsonException($"Unable to parse DateOnly from '{value}'. Expected format: {Format}");
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing DateOnly");
    }

    public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

