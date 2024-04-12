using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetRunAsHandler(IAuthorizationBLLContext authorizationBllContext) : BaseReadHandler, IGetRunAsHandler
{
    protected override Task<object> GetData(HttpContext context) =>
        Task.FromResult<object>(authorizationBllContext.CurrentPrincipal.RunAs?.Name);
}
