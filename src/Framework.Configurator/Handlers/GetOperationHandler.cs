using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.Configurator.Models;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

using NHibernate.Linq;

namespace Framework.Configurator.Handlers;

public class GetOperationHandler(
    IRepositoryFactory<Principal> principalRepoFactory,
    IOperationAccessor operationAccessor,
    ISecurityRoleSource roleSource)
    : BaseReadHandler, IGetOperationHandler
{
    protected override async Task<object> GetDataAsync(HttpContext context, CancellationToken cancellationToken)
    {
        if (!operationAccessor.IsAdministrator()) return new OperationDetailsDto { BusinessRoles = [], Principals = [] };

        var operationName = (string)context.Request.RouteValues["name"]!;

        var roles = roleSource.SecurityRoles
                              .Where(x => x.Information.Operations.Any(o => o.Name == operationName))
                              .ToList();

        var roleIds = roles.Select(x => x.Id).ToList();
        var principals = await principalRepoFactory
                               .Create()
                               .GetQueryable()
                               .Where(x => x.Permissions.Any(p => roleIds.Contains(p.Role.Id)))
                               .Select(x => x.Name)
                               .OrderBy(x => x)
                               .Distinct()
                               .ToListAsync(cancellationToken);

        return new OperationDetailsDto { BusinessRoles = roles.Select(x => x.Name).Order().ToList(), Principals = principals };
    }
}
