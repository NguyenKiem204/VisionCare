using System.Text.Json;
using System.Text.Json.Serialization;

namespace VisionCare.WebAPI.Converters;

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    private const string Format = "HH:mm";

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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

            // Try parsing with HH:mm format first
            if (TimeOnly.TryParseExact(value, Format, out var time))
                return time;

            // Try parsing with HH:mm:ss format
            if (TimeOnly.TryParseExact(value, "HH:mm:ss", out time))
                return time;

            // Try general parsing
            if (TimeOnly.TryParse(value, out time))
                return time;

            throw new JsonException($"Unable to parse TimeOnly from '{value}'. Expected format: {Format} or HH:mm:ss");
        }

        throw new JsonException($"Unexpected token type {reader.TokenType} when parsing TimeOnly");
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(Format));
    }
}

