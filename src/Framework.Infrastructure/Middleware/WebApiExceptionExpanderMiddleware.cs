using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiExceptionExpanderMiddleware
{
    private readonly RequestDelegate next;

    public WebApiExceptionExpanderMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, IWebApiExceptionExpander exceptionExpander)
    {
        try
        {
            await this.next(context);
        }
        catch (TaskCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            var processedEx = exceptionExpander.Process(ex);

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
