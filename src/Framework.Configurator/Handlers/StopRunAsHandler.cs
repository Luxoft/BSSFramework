using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.SecuritySystem.Services;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record StopRunAsHandler(IRunAsManager? RunAsManager = null) : BaseWriteHandler, IStopRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken) =>
        await this.RunAsManager.FromMaybe(() => "RunAs not supported").FinishRunAsUserAsync(cancellationToken);
}
