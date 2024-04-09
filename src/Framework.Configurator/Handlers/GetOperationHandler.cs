using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetOperationHandler : BaseReadHandler, IGetOperationHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetOperationHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
    {
        return new OperationDetailsDto { BusinessRoles = new List<string>(), Principals = new List<string>() };
    }
}
