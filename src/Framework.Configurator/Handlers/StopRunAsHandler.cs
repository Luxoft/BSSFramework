using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class StopRunAsHandler : BaseWriteHandler, IStopRunAsHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public StopRunAsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    public Task Execute(HttpContext context)
    {
        this.authorizationBllContext.Authorization.RunAsManager.FinishRunAsUser();
        return Task.CompletedTask;
    }
}
