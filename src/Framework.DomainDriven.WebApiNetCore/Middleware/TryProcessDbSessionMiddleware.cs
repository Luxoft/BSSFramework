using System;
using System.Threading.Tasks;

using Framework.DomainDriven.BLL;

using Microsoft.AspNetCore.Http;

namespace Framework.DomainDriven.WebApiNetCore;

public sealed class TryProcessDbSessionMiddleware
{
    private readonly RequestDelegate next;

    public TryProcessDbSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context, IServiceProvider serviceProvider)
    {

        try
        {
            return this.next(context);
        }
        catch
        {
            serviceProvider.TryFaultDbSession();
            throw;
        }
        finally
        {
            serviceProvider.TryCloseDbSession();
        }
    }
}
