using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.AspNetCore.Http;

namespace Framework.Infrastructure.Middleware;

public class WebApiExceptionExpanderMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IWebApiExceptionExpander exceptionExpander)
    {
        try
        {
            await next(context);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var processedEx = exceptionExpander.Expand(ex);

            if (processedEx != ex)
            {
                throw processedEx;
            }
            else
            {
                throw;
            }
        }
    }
}
