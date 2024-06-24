using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record RunAsHandler(IRunAsManager RunAsManager) : BaseWriteHandler, IRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principal = await this.ParseRequestBodyAsync<string>(context);
        await this.RunAsManager.StartRunAsUserAsync(principal, cancellationToken);
    }
}
