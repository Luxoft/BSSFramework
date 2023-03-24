using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public abstract class BaseReadHandler : IHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var data = this.GetData(context);
        await context.Response.WriteAsync(JsonSerializer.Serialize(data), cancellationToken);
    }

    protected abstract object GetData(HttpContext context);
}
