using Framework.Authorization.Domain;
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
    ISecurityRoleSource roleSource)
    : BaseReadHandler, IGetBusinessRoleHandler
{
    protected override async Task<object> GetData(HttpContext context)
    {
        if (!operationAccessor.IsAdmin()) return new BusinessRoleDetailsDto { Operations = [], Principals = [] };

        var roleId = new Guid((string)context.Request.RouteValues["id"]!);

        var operations = roleSource.SecurityRoles
                                   .Single(x => x.Id == roleId)
                                   .Operations
                                   .Select(x => new OperationDto { Name = x.Name, Description = x.Description })
                                   .OrderBy(x => x.Name)
                                   .ToList();

        var principals = await repoFactory.Create()
                                          .GetQueryable()
                                          .Where(x => x.Role.Id == roleId)
                                          .Select(x => x.Principal.Name)
                                          .OrderBy(x => x)
                                          .Distinct()
                                          .ToListAsync();

        return new BusinessRoleDetailsDto { Operations = operations, Principals = principals };
    }
}
