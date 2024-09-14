using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRolesHandler(
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    ISecuritySystemFactory securitySystemFactory)
    : BaseReadHandler, IGetBusinessRolesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystemFactory.IsSecurityAdministrator()) return new List<EntityDto>();

        var defaultContexts = securityContextSource.SecurityContextInfoList
                                                   .Select(v => new RoleContextDto(v.Name, false))
                                                   .ToList();

        return securityRoleSource
               .SecurityRoles
               .Select(
                   x => new FullRoleDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsVirtual = x.Information.IsVirtual,
                            Contexts =
                                x.Information.Restriction.SecurityContextRestrictions?.Select(
                                    v => new RoleContextDto(securityContextSource.GetSecurityContextInfo(v.Type).Name, v.Required)).ToList()
                                ?? defaultContexts
                        })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
