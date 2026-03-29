namespace Framework.Infrastructure.Hangfire;

/// <summary>
///
/// </summary>
/// <param name="Name"></param>
/// <param name="Scchedule">Cron. See https://en.wikipedia.org/wiki/Cron for details</param>
public record JobTiming(string Name, string Scchedule);
