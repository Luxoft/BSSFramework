using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Framework.WebApi.Utils;

internal sealed class ClientIpHandling
{
    private readonly RequestDelegate next;

    public ClientIpHandling(RequestDelegate next) => this.next = next;

    public Task Invoke(HttpContext context, ILogger<ClientIpHandling> logger)
    {
        using (logger.BeginScope(new Dictionary<string, object> { [HttpHeaders.ClientIp] = context.Connection.RemoteIpAddress }))
        {
            return this.next(context);
        }
    }
}
