using Framework.Application.Middleware;

using Microsoft.Extensions.Logging;

namespace Framework.Application.Jobs;

public class JobLoggingMiddleware<TJob>(ILogger<TJob> logger) : IScopedEvaluatorMiddleware
{
    public async Task<TResult> EvaluateAsync<TResult>(Func<Task<TResult>> getResult)
    {
        try
        {
            logger.Log(LogLevel.Information, "Job started");

            var result = await getResult();

            logger.Log(LogLevel.Information, "Job finished");

            return result;
        }
        catch (Exception e)
        {
            logger.Log(LogLevel.Error, e, "Job failed");
            throw;
        }
    }
}
