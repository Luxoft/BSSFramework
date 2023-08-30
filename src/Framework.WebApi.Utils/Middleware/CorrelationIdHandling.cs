using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Framework.WebApi.Utils;

internal sealed class CorrelationIdHandling
{
    private readonly RequestDelegate next;

    private readonly string pattern;

    public CorrelationIdHandling(RequestDelegate next, string pattern)
    {
        this.next = next;
        this.pattern = pattern;
    }

    public Task Invoke(HttpContext context, ILogger<CorrelationIdHandling> logger)
    {
        var correlationId = context.Request.Headers[HttpHeaders.Correlation].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = string.Format(this.pattern, Guid.NewGuid());
            context.Request.Headers.Add(HttpHeaders.Correlation, correlationId);
        }

        context.Response.Headers.Add(HttpHeaders.Correlation, correlationId);

        using (logger.BeginScope(new Dictionary<string, object> { [HttpHeaders.Correlation] = correlationId }))
        {
            return this.next(context);
        }
    }
}
