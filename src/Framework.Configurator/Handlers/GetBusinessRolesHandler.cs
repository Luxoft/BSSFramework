using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.ApplicationCore.Security;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRolesHandler(
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    ISecuritySystem securitySystem)
    : BaseReadHandler, IGetBusinessRolesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new List<EntityDto>();

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
