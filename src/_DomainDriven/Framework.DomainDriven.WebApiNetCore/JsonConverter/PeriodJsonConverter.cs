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

        var namingPolicy = options.PropertyNamingPolicy;

        var startDateName = namingPolicy?.ConvertName(nameof(Period.StartDate)) ?? nameof(Period.StartDate);
        var endDateName = namingPolicy?.ConvertName(nameof(Period.EndDate)) ?? nameof(Period.EndDate);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (stringComparer.Equals(propertyName, startDateName))
                {
                    startDate = reader.GetDateTime();
                }
                else if (stringComparer.Equals(propertyName, endDateName))
                {
                    if (reader.TokenType != JsonTokenType.Null)
                    {
                        endDate = reader.GetDateTime();
                    }
                }
            }
        }

        return new Period(startDate, endDate);
    }

    public override void Write(Utf8JsonWriter writer, Period value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var namingPolicy = options.PropertyNamingPolicy;

        var startDateName = namingPolicy?.ConvertName(nameof(Period.StartDate)) ?? nameof(Period.StartDate);
        var endDateName = namingPolicy?.ConvertName(nameof(Period.EndDate)) ?? nameof(Period.EndDate);

        writer.WriteString(startDateName, value.StartDate);
        if (value.EndDate.HasValue)
        {
            writer.WriteString(endDateName, value.EndDate.Value);
        }
        else
        {
            writer.WriteNull(endDateName);
        }

        writer.WriteEndObject();
    }
}
