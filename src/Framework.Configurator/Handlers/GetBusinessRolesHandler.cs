﻿using Framework.Authorization.Environment.Security;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public class GetBusinessRolesHandler(
    ISecurityRoleSource securityRoleSource,
    ISecurityContextInfoService securityContextInfoService,
    IOperationAccessor operationAccessor)
    : BaseReadHandler, IGetBusinessRolesHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsSecurityAdministrator()) return new List<EntityDto>();

        var defaultContexts = securityContextInfoService.SecurityContextTypes.Select(securityContextInfoService.GetSecurityContextInfo)
                                                        .Select(v => new RoleContextDto(v.Name, false))
                                                        .ToList();

        return securityRoleSource
               .GetRealRoles()
               .Select(
                   x => new FullRoleDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Contexts =
                                x.Information.Restriction.SecurityContextRestrictions?.Select(
                                    v => new RoleContextDto(securityContextInfoService.GetSecurityContextInfo(v.Type).Name, v.Required)).ToList()
                                ?? defaultContexts
                        })
               .OrderBy(x => x.Name)
               .ToList();
    }
}
