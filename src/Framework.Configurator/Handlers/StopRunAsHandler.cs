using Framework.Configurator.Interfaces;
using Framework.Core;
using Framework.SecuritySystem.Services;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class StopRunAsHandler(IRunAsManager? runAsManager = null) : BaseWriteHandler, IStopRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken) =>
        await runAsManager.FromMaybe(() => "RunAs not supported").FinishRunAsUserAsync(cancellationToken);
}
