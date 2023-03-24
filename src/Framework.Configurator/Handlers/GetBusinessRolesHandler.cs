using System.Linq;

using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRolesHandler : BaseReadHandler, IGetBusinessRolesHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetBusinessRolesHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context)
        => this.authorizationBllContext.Authorization.Logics.BusinessRoleFactory.Create(BLLSecurityMode.View)
               .GetSecureQueryable()
               .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
               .OrderBy(r => r.Name)
               .ToList();
}
