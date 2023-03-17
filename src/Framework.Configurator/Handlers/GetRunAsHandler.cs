using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetRunAsHandler : BaseReadHandler, IGetRunAsHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetRunAsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context) =>
            this.authorizationBllContext.CurrentPrincipal.RunAs?.Name;
}
