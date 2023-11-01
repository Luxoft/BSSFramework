using Framework.Exceptions;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public class ExceptionExpanderMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionExpanderMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context, IExceptionExpander exceptionExpander)
    {
        try
        {
            await this.next(context);
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
