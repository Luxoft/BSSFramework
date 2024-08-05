using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleSystem.WebApiCore.Json;

public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetDateTime();
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var actualDateTime = value.Kind != DateTimeKind.Utc ? DateTime.SpecifyKind(value, DateTimeKind.Utc) : value;
        writer.WriteStringValue(actualDateTime);
    }
}
