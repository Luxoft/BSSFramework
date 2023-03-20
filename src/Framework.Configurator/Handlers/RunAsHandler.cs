using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record RunAsHandler(IAuthorizationBLLContext AuthorizationBllContext) : BaseWriteHandler, IRunAsHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var principal = await this.ParseRequestBodyAsync<string>(context);
        this.AuthorizationBllContext.Authorization.RunAsManager.StartRunAsUser(principal);
    }
}
