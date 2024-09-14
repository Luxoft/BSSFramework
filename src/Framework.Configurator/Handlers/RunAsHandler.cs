using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.SecuritySystem.Services;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class RunAsHandler(IRunAsManager? runAsManager = null) : BaseWriteHandler, IRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken) =>
        await runAsManager.FromMaybe(() => "RunAs not supported")
                          .StartRunAsUserAsync(await this.ParseRequestBodyAsync<string>(context), cancellationToken);
}
