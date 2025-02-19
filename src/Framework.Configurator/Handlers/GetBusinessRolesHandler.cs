using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.ApplicationSecurity;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRolesHandler(
    ISecurityRoleSource securityRoleSource,
    ISecurityContextInfoSource securityContextInfoSource,
    [CurrentUserWithoutRunAs]ISecuritySystem securitySystem)
    : BaseReadHandler, IGetBusinessRolesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!securitySystem.IsSecurityAdministrator()) return new List<EntityDto>();

        var defaultContexts = securityContextInfoSource.SecurityContextInfoList
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
                                    v => new RoleContextDto(securityContextInfoSource.GetSecurityContextInfo(v.Type).Name, v.Required)).ToList()
                                ?? defaultContexts
                        })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
