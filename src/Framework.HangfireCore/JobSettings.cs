namespace Framework.HangfireCore;

public record JobSettings
{
    public string? Name { get; init; }

    public string? CronTiming { get; init; }

    public string? DisplayName { get; init; }
}
