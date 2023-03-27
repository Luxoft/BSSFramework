using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record StopRunAsHandler(IAuthorizationBLLContext AuthorizationBllContext) : BaseWriteHandler, IStopRunAsHandler
{
    public Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        this.AuthorizationBllContext.Authorization.RunAsManager.FinishRunAsUser();
        return Task.CompletedTask;
    }
}
