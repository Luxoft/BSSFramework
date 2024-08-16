﻿using System.Text.Json.Serialization;
using System.Text.Json;

using Framework.Core;

namespace Framework.DomainDriven.WebApiNetCore.JsonConverter;

public class PeriodJsonConverter : JsonConverter<Period>
{
    public override Period Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var startDate = DateTime.MinValue;
        DateTime? endDate = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();

                if (propertyName == nameof(Period.StartDate))
                {
                    startDate = reader.GetDateTime();
                }
                else if (propertyName == nameof(Period.EndDate))
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