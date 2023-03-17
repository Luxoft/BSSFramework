using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class RunAsHandler : BaseWriteHandler, IRunAsHandler

{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public RunAsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    public async Task Execute(HttpContext context)
    {
        var principal = await this.ParseRequestBodyAsync<string>(context);
        this.authorizationBllContext.Authorization.RunAsManager.StartRunAsUser(principal);
    }
}
