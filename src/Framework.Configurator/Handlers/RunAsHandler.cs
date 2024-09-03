using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.SecuritySystem.Services;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record RunAsHandler(IRunAsManager? RunAsManager = null) : BaseWriteHandler, IRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken) =>
        await this.RunAsManager.FromMaybe(() => "RunAs not supported")
                  .StartRunAsUserAsync(await this.ParseRequestBodyAsync<string>(context), cancellationToken);
}
