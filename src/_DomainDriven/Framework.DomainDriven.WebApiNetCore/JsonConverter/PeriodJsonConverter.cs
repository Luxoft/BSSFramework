#nullable enable

using System.Text.Json.Serialization;
using System.Text.Json;

using Framework.Core;

namespace Framework.DomainDriven.WebApiNetCore.JsonConverter;

public class PeriodJsonConverter : JsonConverter<Period>
{
    public override Period Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var startDate = DateTime.MinValue;
        DateTime? endDate = null;

        var stringComparer = options.PropertyNameCaseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

        var namingPolicy = options.PropertyNamingPolicy ?? DefaultJsonNamingPolicy.Default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (stringComparer.Equals(propertyName, namingPolicy.ConvertName(nameof(Period.StartDate))))
                {
                    startDate = JsonSerializer.Deserialize<DateTime>(ref reader, options);
                }
                else if (stringComparer.Equals(propertyName, namingPolicy.ConvertName(nameof(Period.EndDate))))
                {
                    endDate = JsonSerializer.Deserialize<DateTime?>(ref reader, options);
                }
            }
        }

        return new Period(startDate, endDate);
    }

    public override void Write(Utf8JsonWriter writer, Period value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var namingPolicy = options.PropertyNamingPolicy ?? DefaultJsonNamingPolicy.Default;

        writer.WritePropertyName(namingPolicy.ConvertName(nameof(Period.StartDate)));
        JsonSerializer.Serialize(writer, value.StartDate, options);

        writer.WritePropertyName(namingPolicy.ConvertName(nameof(Period.EndDate)));
        JsonSerializer.Serialize(writer, value.EndDate, options);

        writer.WriteEndObject();
    }
}
