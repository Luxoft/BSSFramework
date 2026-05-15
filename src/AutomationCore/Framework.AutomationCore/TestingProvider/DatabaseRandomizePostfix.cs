namespace Framework.AutomationCore.TestingProvider;

public record DatabaseRandomizePostfix(string Value)
{
    public static DatabaseRandomizePostfix Create() => new($"_{Guid.NewGuid():N}");
}
