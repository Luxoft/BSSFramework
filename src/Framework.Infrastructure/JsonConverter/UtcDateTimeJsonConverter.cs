using System.Text.Json.Serialization;
using System.Text.Json;

namespace Framework.DomainDriven.WebApiNetCore.JsonConverter;

public class UtcDateTimeJsonConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetDateTime();

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var actualDateTime = value.Kind == DateTimeKind.Utc ? value : DateTime.SpecifyKind(value, DateTimeKind.Utc);

        writer.WriteStringValue(actualDateTime);
    }
}
