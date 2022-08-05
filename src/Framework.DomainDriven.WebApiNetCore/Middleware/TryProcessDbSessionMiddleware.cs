using System;
using System.Threading.Tasks;

using Framework.DomainDriven.BLL;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public class TryProcessDbSessionMiddleware
{
    private readonly RequestDelegate next;

    public TryProcessDbSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context)
    {
        try
        {
            return this.next(context);
        }
        catch
        {
            context.RequestServices.TryFaultDbSession();
            throw;
        }
        finally
        {
            context.RequestServices.TryCloseDbSession();
        }
    }
}
