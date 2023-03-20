using System;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class UpdatePrincipalHandler: BaseWriteHandler, IUpdatePrincipalHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public UpdatePrincipalHandler(IAuthorizationBLLContext authorizationBllContext) => this.authorizationBllContext = authorizationBllContext;

    public async Task Execute(HttpContext context)
    {
        var principalId = (string)context.Request.RouteValues["id"] ?? throw new InvalidOperationException();
        var name = await this.ParseRequestBodyAsync<string>(context);

        this.Update(new Guid(principalId), name);
    }

    private void Update(Guid id, string newName)
    {
        var principalBll = this.authorizationBllContext.Authorization.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit);
        var domainObject = principalBll.GetById(id, true);
        domainObject.Name = newName;
        principalBll.Save(domainObject);
    }
}
