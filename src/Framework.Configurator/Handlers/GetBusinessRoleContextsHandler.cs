using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleContextsHandler(
    ISecurityContextSource securityContextSource,
    ISecuritySystemFactory securitySystemFactory)
    : BaseReadHandler, IGetBusinessRoleContextsHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystemFactory.IsSecurityAdministrator()) return new List<EntityDto>();

        return securityContextSource
               .SecurityContextInfoList
               .Select(x => new EntityDto { Id = x.Id, Name = x.Name })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
