using System.Diagnostics;

namespace Framework.Core.Helpers;

/// <summary>
/// Performance Helper
/// </summary>
public static class PerformanceHelper
{
    /// <summary>
    /// Calculates method execution duration (in ms)
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Action duration in miliseconds</returns>
    public static long Duration(Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        action();

        stopwatch.Stop();
        return stopwatch.ElapsedMilliseconds;
    }

    /// <summary>
    /// Executing action and returns health status
    /// </summary>
    /// <param name="action">Action</param>
    /// <returns>Health Status</returns>
    public static string GetHealthStatus(Action action)
    {
        var duration = Duration(action);
        return $"Health status is ok. SQL execution time: {duration} miliseconds";
    }
}
