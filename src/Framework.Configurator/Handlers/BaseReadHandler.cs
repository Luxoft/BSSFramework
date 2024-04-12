using System.Text.Json;

using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public abstract class BaseReadHandler : IHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var data = await this.GetDataAsync(context, cancellationToken);
        await context.Response.WriteAsync(JsonSerializer.Serialize(data), cancellationToken);
    }

    protected abstract Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken);
}
