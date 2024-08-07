﻿using Framework.Authorization.Domain;
using Framework.Authorization.Environment.Security;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetBusinessRoleHandler(
    IRepositoryFactory<Permission> repoFactory,
    IOperationAccessor operationAccessor,
    ISecurityRoleSource roleSource,
    ISecurityOperationInfoSource operationInfoSource)
    : BaseReadHandler, IGetBusinessRoleHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsSecurityAdministrator()) return new BusinessRoleDetailsDto { Operations = [], Principals = [] };

        var roleId = new Guid((string)context.Request.RouteValues["id"]!);

        var operations = roleSource.GetRealRoles()
                                   .Single(x => x.Id == roleId)
                                   .Information
                                   .Operations
                                   .Select(o => new OperationDto { Name = o.Name, Description = operationInfoSource.GetSecurityOperationInfo(o).Description })
                                   .OrderBy(x => x.Name)
                                   .ToList();

        var principals = await repoFactory.Create()
                                          .GetQueryable()
                                          .Where(x => x.Role.Id == roleId)
                                          .Select(x => x.Principal.Name)
                                          .OrderBy(x => x)
                                          .Distinct()
                                          .ToListAsync(cancellationToken);

        return new BusinessRoleDetailsDto { Operations = operations, Principals = principals };
    }
}
