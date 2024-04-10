using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextsHandler : BaseReadHandler, IGetBusinessRoleContextsHandler
{
    private readonly IAuthorizationBLLContext authorizationBllContext;

    public GetBusinessRoleContextsHandler(IAuthorizationBLLContext authorizationBllContext) =>
            this.authorizationBllContext = authorizationBllContext;

    protected override object GetData(HttpContext context) =>
            this.authorizationBllContext.Logics.EntityTypeFactory.Create(SecurityRule.View)
                .GetSecureQueryable()
                .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                .OrderBy(r => r.Name)
                .ToList();
}
