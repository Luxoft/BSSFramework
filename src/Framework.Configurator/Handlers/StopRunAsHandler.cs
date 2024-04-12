using Framework.Authorization.SecuritySystem;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record StopRunAsHandler(IRunAsManager RunAsManager) : BaseWriteHandler, IStopRunAsHandler
{
    public Task Execute(HttpContext context, CancellationToken cancellationToken) =>
        this.RunAsManager.FinishRunAsUserAsync(cancellationToken);
}
