using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextsHandler(
    ISecurityContextInfoService securityContextInfoService,
    IAuthorizationSystem authorizationSystem)
    : BaseReadHandler, IGetBusinessRoleContextsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!authorizationSystem.IsSecurityAdministrator()) return new List<EntityDto>();

        return securityContextInfoService
               .SecurityContextTypes
               .Select(securityContextInfoService.GetSecurityContextInfo)
               .Select(x => new EntityDto { Id = x.Id, Name = x.Name })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
