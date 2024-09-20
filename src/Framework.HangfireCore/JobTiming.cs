namespace Framework.HangfireCore;

/// <summary>
///
/// </summary>
/// <param name="Name"></param>
/// <param name="Schedule">Cron. See https://en.wikipedia.org/wiki/Cron for details</param>
public record JobTiming(string Name, string Schedule);
