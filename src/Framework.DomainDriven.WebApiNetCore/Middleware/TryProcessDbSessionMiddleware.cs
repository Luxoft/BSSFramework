using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public class TryProcessDbSessionMiddleware
{
    private readonly RequestDelegate next;

    public TryProcessDbSessionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context, IWebApiDBSessionModeResolver sessionModeResolver)
    {
        try
        {
            var sessionMode = sessionModeResolver.GetSessionMode();

            if (sessionMode != null)
            {
                var dbSession = context.RequestServices.GetRequiredService<IDBSession>();

                switch (sessionMode)
                {
                    case DBSessionMode.Read:
                        dbSession.AsReadOnly();
                        break;

                    case DBSessionMode.Write:
                        dbSession.AsWritable();
                        break;

                }
            }

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
