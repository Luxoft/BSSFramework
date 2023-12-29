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

    public async Task Invoke(HttpContext context, IDBSessionManager dbSessionManager, IWebApiDBSessionModeResolver sessionModeResolver)
    {
        await dbSessionManager.EvaluateAsync(
            async () =>
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

                await this.next(context);
            },
            context.RequestAborted);
    }
}
