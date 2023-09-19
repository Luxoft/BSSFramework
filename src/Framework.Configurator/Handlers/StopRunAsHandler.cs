using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record StopRunAsHandler(IAuthorizationBLLContext AuthorizationBllContext) : BaseWriteHandler, IStopRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        await this.AuthorizationBllContext.Authorization.RunAsManager.FinishRunAsUserAsync(cancellationToken);
    }
}
