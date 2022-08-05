using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiExceptionExpanderMiddleware
{
    private readonly RequestDelegate next;

    public WebApiExceptionExpanderMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context, IWebApiExceptionExpander exceptionExpander)
    {
        try
        {
            return this.next(context);
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
