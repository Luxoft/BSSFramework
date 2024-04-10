using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetOperationsHandler : BaseReadHandler, IGetOperationsHandler

{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetOperationsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
        => new List<string>();
}
