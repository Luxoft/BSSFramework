

using System.Text.Json;

namespace Framework.DomainDriven.WebApiNetCore.JsonConverter;

public class DefaultJsonNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => name;

    public static DefaultJsonNamingPolicy Default { get; } = new();
}
