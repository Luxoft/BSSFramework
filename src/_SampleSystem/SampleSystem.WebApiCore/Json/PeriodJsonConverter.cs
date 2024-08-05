using System.Text.Json;
using System.Text.Json.Serialization;

using Framework.Core;

namespace SampleSystem.WebApiCore.Json;

public class PeriodJsonConverter : JsonConverter<Period>
{
    public override Period Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        JsonSerializer.Deserialize<Period>(ref reader, options);

    public override void Write(Utf8JsonWriter writer, Period value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(Period.StartDate), value.StartDate);
        if (value.EndDate.HasValue)
        {
            writer.WriteString(nameof(Period.EndDate), value.EndDate.Value);
        }
        else
        {
            writer.WriteNull(nameof(Period.EndDate));
        }

        writer.WriteEndObject();
    }
}
