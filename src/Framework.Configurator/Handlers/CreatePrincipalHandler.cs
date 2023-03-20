using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class CreatePrincipalHandler : BaseWriteHandler, ICreatePrincipalHandler

{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public CreatePrincipalHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    public async Task Execute(HttpContext context)
    {
        var name = await this.ParseRequestBodyAsync<string>(context);
        this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit)
            .Save(new Principal { Name = name });
    }
}
