using Framework.Authorization.BLL;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextsHandler(IAuthorizationBLLContext authorizationBllContext)
    : BaseReadHandler, IGetBusinessRoleContextsHandler
{
    protected override Task<object> GetData(HttpContext context) =>
        Task.FromResult<object>(
            authorizationBllContext.Logics.SecurityContextTypeFactory.Create(SecurityRule.View)
                                   .GetSecureQueryable()
                                   .Select(r => new EntityDto { Id = r.Id, Name = r.Name })
                                   .OrderBy(r => r.Name)
                                   .ToList());
}
